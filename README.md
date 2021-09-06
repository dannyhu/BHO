# BHO

一、如何调试挂起注册

	0.管理员CMD模式下如需要清楚全局GAC缓存,否则无法激活断点！！！！！
	"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.2 Tools\gacutil.exe" /u "$(TargetName)"
	
	1.项目 -> 属性 -> 调试 -> 启动外部程序 -> 设置如下
		
		C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe
	
	2.启动选择 -> 命令行参数输入如下
	
		/codebase "D:\SVN_Pros\BHO\BHO\bin\Debug\BHO.dll"
	
	3.启动选择 -> 命令行参数输入如下
	
		/codebase "D:\SVN_Pros\BHO\BHO\bin\Debug\BHO.dll" /u


二、源代码自动化配置

	1.项目 -> 属性 -> 调试 -> 启动外部程序 -> 设置如下
	
		C:\Program Files (x86)\Internet Explorer\iexplore.exe
	
	2.项目 -> 属性 -> 生成事件 -> 生成后事件命令行 -> 设置如下
	
		"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.2 Tools\gacutil.exe" /u "$(TargetName)"
		"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.2 Tools\gacutil.exe" /f /i "$(TargetPath)"
		"C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe" /unregister /codebase "$(TargetPath)"
		"C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe" /codebase "$(TargetPath)"


三、CMD命令行执行注册与反注册

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe" /codebase "D:\SVN_Pros\BHO\BHO\bin\Debug\BHO.dll"

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe" /codebase "D:\SVN_Pros\BHO\BHO\bin\Debug\BHO.dll" /u