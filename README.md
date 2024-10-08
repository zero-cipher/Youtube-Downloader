
# 유투브 영상 다운로드

## 개발 툴
Tool : Microsoft Visual Studio 2019 Community Edition</br>
Language : C#</br>
.NET Framework : .NET Framework 4.7.2</br>

## 사용 라이브러리
log4net 3.0.1 - The Apache Software Foundation</br>
https://logging.apache.org/log4net/

MediaToolkit 1.1.0.1 - Aydin</br>
https://github.com/AydinAdn/MediaToolkit

VideoLibrary 3.2.6 - Bar Arnon, OMANSAK</br>
https://github.com/omansak/libvideo

WindowsAPICodePackShell 8.0.5 - Microsoft Corporation

## 개발 동기
마눌님이 유투브 영상을 다운로드 받을 일이 있다고 하여 곰플레이어 설치하면 같이 설치되는 유투브 다운로드 프로그램을 쓰라고 했는데,
자꾸 이상한 광고가 뜬다고 하여 만들게 됨.
유투브가 영상 처리하는 방법이 독특해 처음에 당황했는데 어찌어찌 1차 완성이 되었음.

## 환경 설정
프로그램 옵션들을 저장하고 있는 "config.ini"파일이 존재한다.
기본적으로는 SavePath 항목만 존재한다.
사용가능한 항목은 아래와 같다.
- ffmpegoption : 영상 파일과 음성 파일을 다운로드 한 뒤 합치기 위해 MediaToolkit Library를 사용하는데, 해당 라이브러리가 ffmpeg을 사용하는 것으로 확인 되었다.</br>
기본적은 -i, -o 옵션은 프로그램에서 붙여주고 나머지 옵션은 config.ini 파일의 내용으로 채운다. 옵션에 대한 내용은 ffmpeg를 참고하면 된다.</br>
예제) ffmpegoption=-preset veryfast

- VideoQuality : 다운로드 받을 youtube 영상 해상도 옵션이다. 지정하지 않거나 0이면 기본 옵션으로 최고 해상도의 영상을 다운로드 한다.</br>
그 외의 값을 입력하면 720p MP4 영상을 검색하고 있을 경우 해당 영상을 다운로드 한다. 없으면 최고 해상도의 영상을 다운로드한다.</br>
예제) VideoQuality=1

- TargetFileExtension : 최종 저장될 파일의 확장자이다. 마눌님은 ".avi"만 사용하기에 ".avi"로 고정했는데, 원본 형태로 받고 싶은 경우도 있을 거 같아서 추가 함</br>
값을 지정하지 않으면 원본 영상의 확장자명을 그대로 사용한다. 음성만 있을 경우 ".mp4"로 한다.</br>
예제) TargetFileExtension=.avi


## 앞으로 해야할 일들
집에서 테스트 했을 때 일부 컨텐츠 다운로드 중간에 멈추는 현상이 발생했다.
중간에 오류가 발생해서 멈출 경우 이어받기가 가능한 로직을 만들어야 한다.


