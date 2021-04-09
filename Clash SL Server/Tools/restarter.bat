echo off
echo 	888     888  .d8888b.   .d8888b.  
echo 	888     888 d88P  Y88b d88P  Y88b 
echo 	888     888 888    888 Y88b.      
echo 	888     888 888         ""Y888b.   
echo 	888     888 888            ""Y88b. 
echo 	888     888 888    888       ""888 
echo 	Y88b. .d88P Y88b  d88P Y88b  d88P 
echo         "Y88888P"   "Y8888P"   "Y8888P"
echo
color 0B   
echo css-Restart-Op v0.1 by Aidid   
echo.   
echo Your css is being restarted, Please wait. . .   
echo Killing process CSS.exe. . .
timeout /t 300
taskkill /f /im CSS.exe   
cls   
echo Success!   
echo.   
echo Your css is now starting. . .   
timeout /t 15
start CSS.exe   
exit 
