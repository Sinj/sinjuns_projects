#include "CursorControl.h"

using namespace std;

CursorControl::CursorControl(void)
{
}

CursorControl::~CursorControl(void)
{
}

void CursorControl::MouseMove(int x, int y)
{
	//Link to article source: http://forums.codeguru.com/showthread.php?377394-Windows-SDK-User-Interface-How-can-I-emulate-mouse-events-in-an-application
	
	POINT p;
	GetCursorPos(&p);
	x = x + p.x;
	y = y + p.y;
	
	//Gets the size of the width and height - 1.
	double screenWidth = ::GetSystemMetrics(SM_CXSCREEN) - 1;
	double screenHeight = ::GetSystemMetrics(SM_CYSCREEN) - 1;

	double fx = (x * (65535.0f / screenWidth));
	double fy = (y * (65535.0f / screenHeight));

	//Create array of input structures.
	INPUT input = {0};

	input.type = INPUT_MOUSE;
	input.mi.dwFlags = MOUSEEVENTF_MOVE|MOUSEEVENTF_ABSOLUTE;
	input.mi.dx = fx;
	input.mi.dy = fy;

	//(1 = Number of structures in array, 2 = the input structures themselves in an array, 3 = The size of an INPUT structure).
	::SendInput(1, &input, sizeof(INPUT));
}

void CursorControl::LeftClick()
{
	//Creates input structures.
	INPUT Input = {0};

	//Click Down
	Input.type = INPUT_MOUSE;
	Input.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
	::SendInput(1 , &Input, sizeof(INPUT));

	//Click Up
	::ZeroMemory(&Input,sizeof(INPUT));
	Input.type = INPUT_MOUSE;
	Input.mi.dwFlags = MOUSEEVENTF_LEFTUP;
	::SendInput(1, &Input, sizeof(INPUT));
}

void CursorControl::RightClick()
{
	//Creates input structures.
	INPUT Input = {0};

	//Click Down
	Input.type = INPUT_MOUSE;
	Input.mi.dwFlags = MOUSEEVENTF_RIGHTDOWN;
	::SendInput(1 , &Input, sizeof(INPUT));

	//Click Up
	::ZeroMemory(&Input,sizeof(INPUT));
	Input.type = INPUT_MOUSE;
	Input.mi.dwFlags = MOUSEEVENTF_RIGHTUP;
	::SendInput(1, &Input, sizeof(INPUT));
}

void CursorControl::StartLeftDrag()
{
	//Creates input structures.
	INPUT Input = {0};

	//Click Down
	Input.type = INPUT_MOUSE;
	Input.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
	::SendInput(1 , &Input, sizeof(INPUT));
}

void CursorControl::EndLeftDrag()
{
	//Creates input structures.
	INPUT Input = {0};

	//Click Up
	::ZeroMemory(&Input,sizeof(INPUT));
	Input.type = INPUT_MOUSE;
	Input.mi.dwFlags = MOUSEEVENTF_LEFTUP;
	::SendInput(1, &Input, sizeof(INPUT));
}

void CursorControl::Scroll(float amount)
{
	mouse_event(MOUSEEVENTF_WHEEL, 0, 0, amount, 0);
}

//int main()
//{
//	cout<<"WASD Controls to move the mouse. Q for Left Click and E for Right Click."<<endl<<endl;
//
//	char value = 'M';
//
//	while (value != ' ')
//	{
//		cin >> value;
//
//		if (value == 'A' || value == 'a')
//		{
//			POINT p;
//			GetCursorPos(&p);
//			MouseMove(p.x - 10, p.y);
//		}
//		if (value == 'W' || value == 'w')
//		{
//			POINT p;
//			GetCursorPos(&p);
//			MouseMove(p.x, p.y - 10);
//		}
//		if (value == 'S' || value == 's')
//		{
//			POINT p;
//			GetCursorPos(&p);
//			MouseMove(p.x, p.y + 10);
//		}
//		if (value == 'D' || value == 'd')
//		{
//			POINT p;
//			GetCursorPos(&p);
//			MouseMove(p.x + 10, p.y);
//		}
//		if (value == 'Q' || value == 'q')
//		{
//			LeftClick();
//		}
//		if (value == 'E' || value == 'e')
//		{
//			RightClick();
//		}
//	}
//
//	return 0;
//}