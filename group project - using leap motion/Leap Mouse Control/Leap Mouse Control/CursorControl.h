#include <iostream>
#include <Windows.h>

#pragma once

class CursorControl
{
	public:
		CursorControl(void);
		~CursorControl(void);
		void MouseMove(int x, int y);
		void LeftClick();
		void RightClick();
		void StartLeftDrag();
		void EndLeftDrag();
		void Scroll(float amount);
};