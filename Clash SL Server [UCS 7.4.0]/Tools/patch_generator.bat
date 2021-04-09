CSSgflzma.exe ../gamefiles/csv ../patch/gamefiles/csv *.*
rem CSSgflzma.exe ../gamefiles/sc ../patch/gamefiles/sc *.*
xcopy /E ../gamefiles/sc ../patch/gamefiles/sc/*.*
CSSgflzma.exe ../gamefiles/logic ../patch/gamefiles/logic *.*
CSSbuildsha.exe ../patch/gamefiles ../patch ../gamefiles *.*