// WBWBEngine.cpp : 애플리케이션에 대한 진입점을 정의합니다.
//

#include "framework.h"
#include "Editor_Window.h"

#include "..\\WBWBEngine_Source\\wbApplication.h"
#include "..\\WBWBEngine_Source\\wbLoadScenes.h"
#include "..\\WBWBEngine_Window\\wbLoadResources.h"
#include "..\\WBWBEngine_Source\\wbResources.h"
#include "..\\WBWBEngine_Source\\wbTexture.h"
#include "..\\WBWBEngine_Source\\wbTime.h"

#include "..\\WBWBEngine_Window\\wbToolScene.h"
#define MAX_LOADSTRING 100

// 전역 변수:
HINSTANCE hInst;                                // 현재 인스턴스입니다.
WCHAR szTitle[MAX_LOADSTRING];                  // 제목 표시줄 텍스트입니다.
WCHAR szWindowClass[MAX_LOADSTRING];            // 기본 창 클래스 이름입니다.


ULONG_PTR gpToken; // Gdiplus 사용을 위한변수
Gdiplus::GdiplusStartupInput gpsi;

// 이 코드 모듈에 포함된 함수의 선언을 전달합니다:
ATOM                MyRegisterClass(HINSTANCE hInstance, const wchar_t* name, WNDPROC proc);
BOOL                InitInstance(HINSTANCE, int);
LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);
LRESULT CALLBACK    WndTileProc(HWND, UINT, WPARAM, LPARAM);
//INT_PTR CALLBACK    About(HWND, UINT, WPARAM, LPARAM);

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPWSTR    lpCmdLine,
                     _In_ int       nCmdShow)
{
    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);

    // TODO: 여기에 코드를 입력합니다.

    // 전역 문자열을 초기화합니다.
    LoadStringW(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
    LoadStringW(hInstance, IDC_WBWBENGINE, szWindowClass, MAX_LOADSTRING);
    MyRegisterClass(hInstance, szWindowClass, WndProc);
    MyRegisterClass(hInstance, L"TILEWINDOW", WndTileProc);
    
    // 애플리케이션 초기화를 수행합니다:
    if (!InitInstance (hInstance, nCmdShow))
    {
        return FALSE;
    }

    HACCEL hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_WBWBENGINE));

    MSG msg = {};

    while (true)
    {
        if (msg.message == WM_QUIT) break;

        if (PeekMessage(&msg, nullptr, 0, 0, PM_REMOVE))
        {
            if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
            {
                TranslateMessage(&msg);
                DispatchMessage(&msg);
            }
        }
        else
        {
            wb::Application::GetInstance().Run();
        }
    }

    Gdiplus::GdiplusShutdown(gpToken);
    wb::Application::GetInstance().Release();

    return (int) msg.wParam;
}



//
//  함수: MyRegisterClass()
//
//  용도: 창 클래스를 등록합니다.
//
ATOM MyRegisterClass(HINSTANCE hInstance, const wchar_t* name, WNDPROC proc)
{
    WNDCLASSEXW wcex;

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style          = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc    = proc;
    wcex.cbClsExtra     = 0;
    wcex.cbWndExtra     = 0;
    wcex.hInstance      = hInstance;
    wcex.hIcon          = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_WBWBENGINE));
    wcex.hCursor        = LoadCursor(nullptr, IDC_ARROW);
    wcex.hbrBackground  = (HBRUSH)(COLOR_WINDOW+1);
    wcex.lpszMenuName   = nullptr;
    wcex.lpszClassName  = name;
    wcex.hIconSm        = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

    return RegisterClassExW(&wcex);
}

//
//   함수: InitInstance(HINSTANCE, int)
//
//   용도: 인스턴스 핸들을 저장하고 주 창을 만듭니다.
//
//   주석:
//
//        이 함수를 통해 인스턴스 핸들을 전역 변수에 저장하고
//        주 프로그램 창을 만든 다음 표시합니다.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
   hInst = hInstance; // 인스턴스 핸들을 전역 변수에 저장합니다.

   const UINT width = 640;
   const UINT height = 480;

   HWND hWnd = CreateWindowW(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
       CW_USEDEFAULT, 0, width, height, nullptr, nullptr, hInstance, nullptr);

   HWND ToolHwnd = CreateWindowW(L"TILEWINDOW", L"TileWindow", WS_OVERLAPPEDWINDOW,
	   0, 0, CW_USEDEFAULT, 0, nullptr, nullptr, hInstance, nullptr);

   wb::Application::GetInstance().Initialize(hWnd, width, height);
   if (!hWnd)
   {
      return FALSE;
   }

   ShowWindow(hWnd, nCmdShow);
   UpdateWindow(hWnd); 
   
   
   Gdiplus::GdiplusStartup(&gpToken, &gpsi, nullptr);

   srand((UINT)time(0));
   wb::LoadResources();
   wb::LoadScenes();


   wb::Texture* texture
	   = wb::Resources::Find<wb::Texture>(L"MAP1");
   RECT rect = { 0,0,texture->GetWidth(),texture->GetHeight() };
   AdjustWindowRect(&rect, WS_OVERLAPPEDWINDOW, false);

   UINT toolWidth = rect.right - rect.left;
   UINT toolHeight = rect.bottom - rect.top;
   SetWindowPos(ToolHwnd, nullptr, width+ 10, 0, toolWidth, toolHeight, 0);
   ShowWindow(ToolHwnd, nCmdShow);
   UpdateWindow(ToolHwnd);
   return TRUE;
}

//
//  함수: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  용도: 주 창의 메시지를 처리합니다.
//
//  WM_COMMAND  - 애플리케이션 메뉴를 처리합니다.
//  WM_PAINT    - 주 창을 그립니다.
//  WM_DESTROY  - 종료 메시지를 게시하고 반환합니다.
//
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
    case WM_COMMAND:
        {
            int wmId = LOWORD(wParam);
            // 메뉴 선택을 구문 분석합니다:
            switch (wmId)
            {
           /* case IDM_ABOUT:
                DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
                break;*/
            case IDM_EXIT:
                DestroyWindow(hWnd);
                break;
            default:
                return DefWindowProc(hWnd, message, wParam, lParam);
            }
        }
        break;
    case WM_PAINT:
        {
            PAINTSTRUCT ps;
            HDC hdc = BeginPaint(hWnd, &ps);
            
           /* HBRUSH blueBrush = CreateSolidBrush(RGB(0, 0, 255));

            HBRUSH oldBrush = (HBRUSH)SelectObject(hdc, blueBrush);
            Rectangle(hdc, 200, 200, 300, 300);
            SelectObject(hdc, oldBrush);

            DeleteObject(blueBrush);

            HPEN redPen = CreatePen(PS_SOLID, 2, RGB(255, 0, 0));
            HPEN oldPen = (HPEN)SelectObject(hdc, redPen);
            
            Ellipse(hdc, 200, 200, 300, 300);

            SelectObject(hdc, oldPen);
            DeleteObject(redPen);*/
            
           /* HBRUSH grayBrush = (HBRUSH)GetStockObject(GRAY_BRUSH);
            oldBrush = (HBRUSH)SelectObject(hdc, grayBrush);
            Rectangle(hdc, 400, 400, 500, 500);
            SelectObject(hdc, oldBrush);*/
            // TODO: 여기에 hdc를 사용하는 그리기 코드를 추가합니다...
            EndPaint(hWnd, &ps);
        }
        break;
    case WM_DESTROY:
        PostQuitMessage(0);
        break;
    /*case WM_MOVE:
    {
        SetWindowText(hWnd, L"WM_MOVE");
    }
    break;

    case WM_SIZE:
    {
        SetWindowText(hWnd, L"WM_SIZE");
    }
    break;
    case WM_LBUTTONDOWN:
    {
        SetWindowText(hWnd, L"LBUTTONDOWN");
    }
    break;
    case WM_RBUTTONDOWN:
    {
        SetWindowText(hWnd, L"RBUTTONDOWN");
    }
    break;
    case WM_KEYDOWN:
    {
        SetWindowText(hWnd, L"WM_KEYDOWN");
    }
    break;*/
   /* case WM_VSCROLL:
    {
        SetWindowText(hWnd, L"WM_VSCROLL");
    }
    break;
    case WM_HSCROLL:
    {
        SetWindowText(hWnd, L"WM_HSCROLL");
    }
    break;*/
    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}


//// 정보 대화 상자의 메시지 처리기입니다.
//INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
//{
//    UNREFERENCED_PARAMETER(lParam);
//    switch (message)
//    {
//    case WM_INITDIALOG:
//        return (INT_PTR)TRUE;
//
//    case WM_COMMAND:
//        if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
//        {
//            EndDialog(hDlg, LOWORD(wParam));
//            return (INT_PTR)TRUE;
//        }
//        break;
//    }
//    return (INT_PTR)FALSE;
//}
