#include "CursorControl.h"
#include <iostream>
#include "Leap.h"

using namespace Leap;

class SampleListener : public Listener
{
	public:
		virtual void onInit(const Controller&);
		virtual void onConnect(const Controller&);
		virtual void onDisconnect(const Controller&);
		virtual void onExit(const Controller&);
		virtual void onFrame(const Controller&);
		virtual void onFocusGained(const Controller&);
		virtual void onFocusLost(const Controller&);

		float previousYPrimary;
		float previousYSecondary;

		CursorControl cursor;
		float mouseSpeed;
		bool dragging;
		float primaryHand;
};

void SampleListener::onInit(const Controller& controller)
{
	cursor = CursorControl();
	previousYPrimary = 0;
	previousYSecondary = 0;

	mouseSpeed = 2;
	dragging = false;
	primaryHand = 0;

	controller.setPolicyFlags(Controller::POLICY_BACKGROUND_FRAMES);
	std::cout << "Initialized" << std::endl;
}

void SampleListener::onConnect(const Controller& controller)
{
	std::cout << "Connected" << std::endl;
}

void SampleListener::onDisconnect(const Controller& controller)
{
	//Note: not dispatched when running in a debugger.
	std::cout << "Disconnected" << std::endl;
}

void SampleListener::onExit(const Controller& controller)
{
	std::cout << "Exited" << std::endl;
}

void SampleListener::onFrame(const Controller& controller)
{
	// Get the most recent frame and report some basic information
	const Frame frame = controller.frame();

	system("cls");

	std::cout <<"X: "<< frame.hands().rightmost().palmNormal().x<<std::endl;
	std::cout <<"Y: "<< frame.hands().rightmost().palmNormal().y<<std::endl;
	std::cout <<"Z: "<< frame.hands().rightmost().palmNormal().z<<std::endl<<std::endl;
	std::cout <<"X: "<< frame.hands().rightmost().palmPosition().x<<std::endl;
	std::cout <<"Y: "<< frame.hands().rightmost().palmPosition().y<<std::endl;
	std::cout <<"Z: "<< frame.hands().rightmost().palmPosition().z<<std::endl<<std::endl;

	float primaryHandX = 0;
	float primaryHandZ = 0;
	float primaryHandY = 0;

	float primaryPalmPositionY = 0;

	float secondaryPalmPositionY = 0;

	if (primaryHand == 0)
	{
		//Right Hand Primary
		primaryHandX = frame.hands().rightmost().palmNormal().x;
		primaryHandZ = frame.hands().rightmost().palmNormal().z;
		primaryHandY = frame.hands().rightmost().palmNormal().y;

		primaryPalmPositionY = frame.hands().rightmost().palmPosition().y;

		//Left Hand Secondary
		secondaryPalmPositionY = frame.hands().leftmost().palmPosition().y;
	}
	else 
	{
		//Left Hand Primary
		primaryHandX = frame.hands().leftmost().palmNormal().x;
		primaryHandZ = frame.hands().leftmost().palmNormal().z;
		primaryHandY = frame.hands().leftmost().palmNormal().y;

		primaryPalmPositionY = frame.hands().leftmost().palmPosition().y;

		//Right Hand Secondary
		secondaryPalmPositionY = frame.hands().rightmost().palmPosition().y;
	}

	if (frame.hands().count() == 1)
	{
		//Dragging Check
	/*if (dragging == true)
	{
		dragging = false;
		cursor.EndLeftDrag();
	}*/

		if (primaryHandX > 0.4f)
		{
			std::cout<< "Gesturing Left"<<std::endl;
			cursor.MouseMove((-(primaryHandX - 0.2f) / 2) * 100, 0);
		}

		if (primaryHandX < -0.4f)
		{
			std::cout<< "Gesturing Right"<<std::endl;
			cursor.MouseMove((-(primaryHandX - -0.2f) / 2) * 100, 0);
		}

		if (primaryHandZ > 0.2f)
		{
			std::cout<< "Gesturing Forwards"<<std::endl;
			cursor.MouseMove(0, ((-(primaryHandZ - 0.1f) / 2) * 100));
		}

		if (primaryHandZ < -0.6f)
		{
			std::cout<< "Gesturing Backwards"<<std::endl;
			cursor.MouseMove(0, ((-(primaryHandZ - -0.3f) / 2) * 100));
		}

		if ((previousYPrimary - primaryPalmPositionY) > 50 && previousYPrimary != 0)
		{
			std::cout<< "Gesturing Left Click"<<std::endl;
			cursor.LeftClick();
		}

		if ((previousYPrimary - primaryPalmPositionY) < -75 && previousYPrimary != 0)
		{
			std::cout<< "Gesturing Right Click"<<std::endl;
			cursor.RightClick();
		}
	}
	else if(frame.hands().count() == 2)
	{
		bool doubleClick = false;

		if ((previousYPrimary - primaryPalmPositionY) * 1 > 50 && previousYPrimary != 0 &&  (previousYSecondary - secondaryPalmPositionY) * 1 > 50 && previousYSecondary != 0)
		{
			std::cout<< "Gesturing Double Click"<<std::endl;
			cursor.LeftClick();
			cursor.LeftClick();
			doubleClick = true;
		}
		else if (primaryHandZ > 0.2f)
		{
			std::cout<< "Gesturing Scroll Up"<<std::endl;
			cursor.Scroll(75);
		}
		else if (primaryHandZ < -0.4f)
		{
			std::cout<< "Gesturing Scroll Down"<<std::endl;
			cursor.Scroll(-75);
		}
		else if ((previousYPrimary - primaryPalmPositionY) < -75 && previousYPrimary != 0)
		{
			std::cout<< "Gesturing Left Drag End"<<std::endl;
			cursor.EndLeftDrag();
		} 
		else if ((previousYPrimary - primaryPalmPositionY) > 50 && previousYPrimary != 0)
		{
			if (doubleClick == true)
				doubleClick = false;
			else
			{
				std::cout<< "Gesturing Left Drag"<<std::endl;

				dragging = true;
				cursor.StartLeftDrag();
			}
		}
	}

	previousYSecondary = secondaryPalmPositionY;
	previousYPrimary = primaryPalmPositionY;

	return;
}

void SampleListener::onFocusGained(const Controller& controller)
{
	std::cout << "Focus Gained" << std::endl;
}

void SampleListener::onFocusLost(const Controller& controller)
{
	std::cout << "Focus Lost" << std::endl;
}

int main()
{
	// Create a sample listener and controller
	SampleListener listener;
	Controller controller;

	//Initialise Users Settings, Choices and Options
	std::cout << "Are you left handed or right handed? INPUT: 'L' or 'R': ";

	char userHand = std::cin.get();

	while (userHand != 'l' || userHand != 'L' || userHand != 'r' || userHand != 'R')
	{
		if (userHand == 'L' || userHand == 'l')
		{
			listener.primaryHand = 1;
			break;
		}
		else if (userHand == 'R' || userHand == 'r')
		{
			listener.primaryHand = 0;
			break;
		}
		else
		{
			std::cout << "Incorrect Input Detected. Please try again." << std::endl;
		}

		std::cout << "Are you left handed or right handed? INPUT: 'L' or 'R': ";
		userHand = std::cin.get();
	}

	// Have the sample listener receive events from the controller
	controller.addListener(listener);
  
	// Keep this process running until Enter is pressed
	std::cout << "Press Enter to quit..." << std::endl;
	std::cin.get();
	std::cin.get();
	// Remove the sample listener when done
	controller.removeListener(listener);

	return 0;
}