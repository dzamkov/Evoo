#include <irrlicht.h>

using namespace irr;

using namespace core;
using namespace scene;
using namespace video;
using namespace io;
using namespace gui;

#ifdef _IRR_WINDOWS_
#pragma comment(lib, "Irrlicht.lib")
#pragma comment(linker, "/subsystem:windows /ENTRY:mainCRTStartup")
#endif



int main()
{
	IrrlichtDevice *device = createDevice( video::EDT_DIRECT3D9, dimension2d<u32>(640, 480), 16,
			false, false, false, 0);

	if (!device)
		return 1;

	device->setWindowCaption(L"Evoo");


	IVideoDriver* driver = device->getVideoDriver();
	ISceneManager* smgr = device->getSceneManager();
	IGUIEnvironment* guienv = device->getGUIEnvironment();

	while(device->run())
	{
		driver->beginScene(true, true, SColor(0,0,0,0));
		smgr->drawAll();
		guienv->drawAll();
		driver->endScene();
	}
	
	device->drop();

	return 0;
}
