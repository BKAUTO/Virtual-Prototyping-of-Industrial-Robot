// ConsoleApplication1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <ctime>
#include "cohanddriver.h"
#include <conio.h>
#include <iostream>
#include <winsock2.h>
#include <string>
#include <WS2tcpip.h>

#pragma comment(lib, "ws2_32.lib")

using namespace std;
void Delay(int time)
{
	clock_t now = clock();

	while (clock() - now < time);
}

void C1_Controller(CohandDriver &_test)
{
	int speed = 1000;
	int current = 1000;
	//手指运行到全开状态，姿态处于平行状态
	_test.Finger1ToTarget(0, speed, current);
	_test.Finger2ToTarget(0, speed, current);
	_test.Finger3ToTarget(0, speed, current);
	_test.PoseToTarget(0, speed, current);
	_test.ExcuteCmd();
	Delay(2000);
	while (1)
	{
		//姿态处于平行状态
		_test.PoseToParallelism(speed);
		_test.ExcuteCmd();
		Delay(2000);
		//姿态处于三指相对状态
		_test.PoseToSymmetry(speed);
		_test.ExcuteCmd();
		Delay(2000);
		//手指运行到全闭合状态
		_test.Finger1ToTarget(500, speed, current);
		_test.Finger2ToTarget(500, speed, current);
		_test.Finger3ToTarget(500, speed, current);
		_test.ExcuteCmd();
		Delay(2000);
		//手指运行到全开状态
		_test.Finger1ToTarget(0, speed, current);
		_test.Finger2ToTarget(0, speed, current);
		_test.Finger3ToTarget(0, speed, current);
		_test.ExcuteCmd();
		Delay(2000);
		//姿态处于重合状态--可模拟二指手
		_test.PoseToLine(speed);
		_test.ExcuteCmd();
		Delay(2000);
		//姿态处于平行状态
		_test.PoseToParallelism(speed);
		_test.ExcuteCmd();
		Delay(2000);
		//手指运行到全闭合状态
		_test.Finger1ToTarget(1000, speed, current);
		_test.Finger2ToTarget(1000, speed, current);
		_test.Finger3ToTarget(1000, speed, current);
		_test.ExcuteCmd();
		Delay(2000);
		//手指运行到全开状态
		_test.Finger1ToTarget(0, speed, current);
		_test.Finger2ToTarget(0, speed, current);
		_test.Finger3ToTarget(0, speed, current);
		_test.ExcuteCmd();
		Delay(2000);
	}
}

void B2_Controller(CohandDriver &_test)//二指手控制
{
	int speed = 1000;
	int current = 1000;
	int ch;
	//手指运行到全开状态，姿态处于平行状态
	_test.Finger1ToTarget(0, speed, current);
	_test.ExcuteCmd();
	Delay(2000);
	while (1)
	{
		if (_kbhit()) {//如果有按键按下，则_kbhit()函数返回真
			ch = _getch();//使用_getch()函数获取按下的键值
			std::cout << ch << std::endl;
			if (ch == 106) {//手指运行到全闭合状态
				_test.Finger1ToTarget(1000, speed, current);
				_test.ExcuteCmd();
			}
			else if (ch == 107) {//手指运行到半闭合状态
				_test.Finger1ToTarget(500, speed, current);
				_test.ExcuteCmd();
			}
			else if (ch == 108) {//手指运行到全开状态
				_test.Finger1ToTarget(0, speed, current);
				_test.ExcuteCmd();
			}
		}
		/*//手指运行到半闭合状态
		_test.Finger1ToTarget(500, speed, current);
		_test.ExcuteCmd();
		Delay(2000);
		//手指运行到全开状态
		_test.Finger1ToTarget(0, speed, current);
		_test.ExcuteCmd();
		Delay(2000);
		//手指运行到全闭合状态
		_test.Finger1ToTarget(1000, speed, current);
		_test.ExcuteCmd();
		Delay(2000);
		//手指运行到全开状态
		_test.Finger1ToTarget(0, speed, current);
		_test.ExcuteCmd();
		Delay(2000);*/
	}
}

int main()
{
	CohandDriver test;
	std::vector<uint32_t> info;
	WSADATA wsaData;
	int iRet = 0;
	iRet = WSAStartup(MAKEWORD(2, 2), &wsaData);

	int res = test.OpenPort(115200);
	//std::cout << "hhhhhhhhhhh" << std::endl;
	//加载套接字库
	if (iRet != 0)
	{
		std::cout << "WSAStartup(MAKEWORD(2, 2), &wsaData) execute failed!" << std::endl;
		return -1;
	}
	if (2 != LOBYTE(wsaData.wVersion) || 2 != HIBYTE(wsaData.wVersion))
	{
		WSACleanup();
		std::cout << "WSADATA version is not correct!" << std::endl;
		return -1;
	}
	//std::cout << "hhhhhhhhhhh" << std::endl;
	//创建套接字
	SOCKET clientSocket = socket(AF_INET, SOCK_STREAM, 0);
	if (clientSocket == INVALID_SOCKET)
	{
		std::cout << "clientSocket = socket(AF_INET, SOCK_STREAM, 0) execute failed!" << std::endl;
		return -1;
	}
	//std::cout << "hhhhhhhhhhh" << std::endl;
	//初始化服务器端地址族变量
	SOCKADDR_IN srvAddr;
	//srvAddr.sin_addr.S_un.S_addr = inet_pton("127.0.0.1");
	inet_pton(AF_INET, "192.168.2.104", &srvAddr.sin_addr);
	srvAddr.sin_family = AF_INET;
	srvAddr.sin_port = htons(8088);
	
	//std::cout << "hhhhhhhhhhh" << std::endl;
	//连接服务器
	iRet = connect(clientSocket, (SOCKADDR*)&srvAddr, sizeof(SOCKADDR));
	if (0 != iRet)
	{
		std::cout << "connect(clientSocket, (SOCKADDR*)&srvAddr, sizeof(SOCKADDR)) execute failed!" << std::endl;
		return -1;
	}

	std::cout << "开始接收消息" << std::endl;

	while (res == 1) {
		//接收消息
		char recvBuf[100];
		recv(clientSocket, recvBuf, 100, 0);
		printf("%s\n", recvBuf);
		string str(recvBuf);
		string::const_iterator it = str.begin();
		cout << *it << endl;
		
		if (str[0] == 49) {
			test.Finger1ToTarget(1000, 1000, 1000);
			test.ExcuteCmd();
		}
		if (str[0] == 48) {
			test.Finger1ToTarget(0, 1000, 1000);
			test.ExcuteCmd();
		}
	}
	/*//发送消息
	char sendBuf[100];
	sprintf_s(sendBuf, "Hello, This is client %s", "兔子");
	send(clientSocket, sendBuf, strlen(sendBuf) + 1, 0);*/

	/*if (res == 1)
	{
	int ii = 10;
	while (ii--)
	{
	info.clear();
	test.GetHandInfo(info);
	printf("固件版本号: %d.\n柔性手类型： %d.\n序列号： %d.\n", info[0], info[1], info[2]);
	//B2_Controller(test);
	info.clear();
	test.GetFinger1State(info);
	printf("当前位置: %d.\n当前速度： %d.\n当前力矩： %d.\n", info[0], info[1], info[2]);
	Delay(2000);
	B2_Controller(test);
	}
	}*/
	return 0;
}

