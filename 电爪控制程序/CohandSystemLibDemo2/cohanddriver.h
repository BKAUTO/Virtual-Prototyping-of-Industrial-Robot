#ifndef COHANDDRIVER_H
#define COHANDDRIVER_H
#include <vector>
#include <string>
class CohandSystem;

class CohandDriver
{
public:
	CohandDriver(void);
	~CohandDriver();
	bool OpenPort(std::string _PortName, uint32_t _PortSpeed = 115200);
	int8_t OpenPort(uint32_t _PortSpeed = 115200);
	void ClosePort();

	void Finger1ToTarget(uint32_t _Target, uint32_t _Speed, uint32_t _Cur);
	void Finger2ToTarget(uint32_t _Target, uint32_t _Speed, uint32_t _Cur);
	void Finger3ToTarget(uint32_t _Target, uint32_t _Speed, uint32_t _Cur);
	void PoseToTarget(uint32_t _Target, uint32_t _Speed, uint32_t _Cur);

	void PoseToParallelism(uint32_t _Speed);
	void PoseToSymmetry(uint32_t _Speed);
	void PoseToLine(uint32_t _Speed);
	bool ExcuteCmd();

	bool ResetAllPara();

	void GetHandInfo(std::vector<uint32_t> &_info);
	bool GetFinger1State(std::vector<uint32_t> &_state);
	bool GetFinger2State(std::vector<uint32_t> &_state);
	bool GetFinger3State(std::vector<uint32_t> &_state);
	bool GetPoseState(std::vector<uint32_t> &_state);
private:
	CohandSystem *CoSys;
};

#endif // COHANDSYSTEM_H
