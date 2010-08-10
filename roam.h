
namespace evoo {
	
	/// Implements the data structure for a ROAM terrain (Described here http://www.cognigraph.com/ROAM_homepage/ROAM2/). The terrain (which is actually
	/// a large plain filled with data) can be navigated and have data stored into its vertices and edges.
	template <class E, class V>
	class ROAM {
	private:
		struct _diamond;

	public:
		class VertexRef;
		class EdgeRef;

	public:
		ROAM(V InitialVertices[4], E InitialEdges[4], E CentralEdge) {
			// Root diamond (this diamond encompasses the entire terrain, no more, no less).
			this->_root = new _diamond();
			this->_root->EdgeData = CentralEdge;
			
			// Greater diamonds (which are bigger than the root, but ignore that) contain information
			// about the initial vertices
			_diamond*	greater_diamonds[4];
			for(int t = 0; t < 4; t++) {
				greater_diamonds[t] = new _diamond();
				greater_diamonds[t]->VertexData = InitialVertices[t];
				greater_diamonds[t]->Children[0] = this->_root;
				this->_root->Ancestors[t] = greater_diamonds[t];
			}
			this->_root->ParentARelation = 0;
			this->_root->ParentBRelation = 0;

			// Diamonds smaller one than the root (Contain the edges of the bounded terrain)
			_diamond*	lesser_diamonds[4];
			for(int t = 0; t < 4; t++) {
				lesser_diamonds[t] = new _diamond();
				lesser_diamonds[t]->EdgeData = InitialEdges[t];
				this->_root->Children[t] = lesser_diamonds[t];
			}
		}

		/// Creates references to the 4 initial vertices that make up the terrain.
		void	InitialVertices(VertexRef* Result) {
			for(int t = 0; t < 4; t++) {
				Result[t] = VertexRef(this->_root->Ancestors[t]);
			}
		}

		/// Reference to a vertex in a ROAM terrain. Can be used for navigation.
		class VertexRef {
		public:
			VertexRef() {
				this->_ref = NULL;
			}

			/// Gets the edges that are connected to this vertex. An array of 8 edge references are set. no change indicates
			/// that there is no edge going from the vertex at the direction.
			void	Edges(EdgeRef* Result) {
				for(int t = 0; t < 4; t++) {
					_diamond* d = this->_ref->GetSubDiamond(t);
					if (d != NULL) {
						EdgeRef er;
						er._ref = d;
						Result[t * 2] = er;
					}

					_diamond* c = this->_ref->Children[t];
					if (c != NULL) {
						if(c->ParentA != this->_ref && c->ParentB != this->_ref) {
							EdgeRef er;
							er._ref = c;
							Result[(t * 2) + 1] = er;
						}
					}
				}
			}

			/// Gets the data for the vertex.
			V			VertexData() {
				return this->_ref->VertexData;
			}

			/// Sets the data for the vertex.
			void		SetVertexData(const V& Data) {
				this->_ref->VertexData = Data;
			}

			bool		IsNull() {
				return this->_ref == NULL;
			}

		private:
			VertexRef(_diamond* Ref) {
				this->_ref = Ref;
			}

			friend class ROAM;
			friend class EdgeRef;
			_diamond*	_ref;
		};

		/// Reference to an edge in a ROAM terrain.
		class EdgeRef {
		public:
			EdgeRef() {
				this->_ref = NULL;
			}

			/// Gets the source vertex for the edge, which stays the same for the life of
			/// the edge and can be used for setting the direction of the edge.
			VertexRef	GetSource() {
				VertexRef vr;
				vr._ref = this->_ref->Ancestor;
				return vr;
			}
			
			/// Gets the vertex this edge goes to that isnt the source.
			VertexRef	GetSink() {
				VertexRef vr;
				vr._ref = this->_ref->OlderAncestor;
				return vr;
			}

			/// Gets the data for the edge.
			E			EdgeData() {
				return this->_ref->EdgeData;
			}

			/// Sets the data for the edge.
			void		SetEdgeData(const E& Data) {
				this->_ref->EdgeData = Data;
			}

			bool	IsNull() {
				return this->_ref == NULL;
			}

		private:
			EdgeRef(_diamond* Ref) {
				this->_ref = Ref;
			}

			friend class VertexRef;
			_diamond*	_ref;
		};
		

	private:
		struct _diamond {
			union {
				_diamond*		Ancestors[4];
				struct {
					_diamond*	Ancestor;
					_diamond*	ParentA;
					_diamond*	OlderAncestor;
					_diamond*	ParentB;
				};
			};
			_diamond*		Children[4];
			unsigned char	ParentARelation;
			unsigned char	ParentBRelation;
			E				EdgeData;
			V				VertexData;

			/// Gets the diamond bordering this one at the specified index. The child at
			/// the index would have this as a parent along with the border.
			_diamond*	GetBorder(unsigned char Index) {
				return (Index < 2 ? 
					(this->ParentA->Children[((Index * 2) + this->ParentARelation + 1) % 4]) :
					(this->ParentB->Children[((Index * 2) + this->ParentBRelation + 1) % 4]));
			}

			/// Gets a diamond, a quater the size of the current one with the same orientation that
			/// shares the specified ancestor.
			_diamond*	GetSubDiamond(unsigned char Index) {
				int strange_sequence[4] = {1, 3, 2, 0};
				int other_sequence[4] = {1, 0, 2, 0};
				_diamond* partial = this->Children[Index];
				if (partial != NULL) {
					return partial->Children[strange_sequence[Index]];
				} else {
					partial = this->Children[(Index - 1) % 4];
					if(partial != NULL) {
						return partial->Children[other_sequence[Index]];
					} else {
						return NULL;
					}
				}
			}
		};

		_diamond*	_root;
	};
}