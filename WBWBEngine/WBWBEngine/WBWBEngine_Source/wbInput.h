#pragma once
#include "CommonInclude.h"

enum class eKeyCode
{
	Q,W,E,R,T,Y,U,I,O,P,
	A,S,D,F,G,H,J,K,L,
	Z,X,C,V,B,N,M,
	Left,Right,Up,Down,
	SPACE,LButton,MButton,RButton,
	END
};

enum class eKeyState
{
	Pressed,
	Up,
	Down,
	None
};

namespace wb
{
	class Input
	{
	public:
		struct Key
		{
			eKeyCode keycode;
			eKeyState state;
			bool bPreesed;
		};
		Input();
		~Input();

		static void Initialize();
		static void Update();
		static void LateUpdate();

		static bool GetKey(eKeyCode keycode) { return mKeys[(UINT)keycode].state == eKeyState::Pressed; }
		static bool GetKeyDown(eKeyCode keycode) { return mKeys[(UINT)keycode].state == eKeyState::Down; }
		static bool GetKeyUp(eKeyCode keycode) { return mKeys[(UINT)keycode].state == eKeyState::Up; }

		static Vector2 GetMousePosition() { return mMousePosition; }
		static bool isKeyDown(eKeyCode code);

	private:
		static void createKeys();
		static void updateKeys();
		static void updateKey(Input::Key& key);
		static void updateKeyDown(Input::Key& key);
		static void updateKeyUp(Input::Key& key);
		static void getMousePositionByWindow();
		static void clearKeys();
	private:
		static std::vector<Key> mKeys;
		static Vector2 mMousePosition;
	};
}