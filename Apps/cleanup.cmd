@ECHO OFF
@SET CurFolder=%CD%
@ECHO Cleaning up folder %CurFolder%
@REM Main Folder
@REM @echo checking %CurFolder%\.vs
@IF exist %CurFolder%\.vs (
	@ECHO RD /S /Q %CurFolder%\.vs
	@RD /S /Q %CurFolder%\.vs
)
@REM @echo checking %CurFolder%\bin
@IF exist %CurFolder%\bin (
	@ECHO RD /S /Q %CurFolder%\%%x\bin
	@RD /S /Q %CurFolder%\bin
)

@FOR /f "tokens=*" %%F IN ('dir /b /ad') DO (
	@SET CurFolder=%%F
	@REM @echo checking %CurFolder%\obj
	@IF exist %CurFolder%\obj (
		@ECHO RD /S /Q %CurFolder%\%%x\obj
		@RD /S /Q %CurFolder%\obj
	)
	@REM Sub Directory
	@for /F %%x in ('dir /B /AD %CurFolder%') do (
		@REM @echo checking %CurFolder%\%%x\.vs
		@IF exist %CurFolder%\%%x\.vs (
			@ECHO RD /S /Q %CurFolder%\.vs
			@RD /S /Q %CurFolder%\.vs
		)

		@REM @echo checking %CurFolder%\%%x\bin
		@IF exist %CurFolder%\%%x\bin (
			@ECHO RD /S /Q %CurFolder%\%%x\bin
			@RD /S /Q %CurFolder%\%%x\bin
		)
		@IF exist %CurFolder%\%%x\obj (
			@ECHO RD /S /Q %CurFolder%\%%x\obj
			@RD /S /Q %CurFolder%\%%x\obj
		)
		
	)
)

PAUSE