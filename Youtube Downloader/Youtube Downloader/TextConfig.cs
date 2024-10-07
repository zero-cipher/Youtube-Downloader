using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Youtube_Downloader
{
    class TextConfig
    {
        /// <summary>
        /// 초기화 파일의 지정된 섹션에서 문자열을 검색합니다.
        /// </summary>
        /// <param name="appName">키 이름을 포함하는 섹션의 이름입니다. 이 매개 변수가 NULL인 경우 GetPrivateProfileString 함수는 파일의 모든 섹션 이름을 제공된 버퍼에 복사합니다.</param>
        /// <param name="keyName">연결된 문자열을 검색할 키의 이름입니다. 이 매개 변수가 NULL이면 lpAppName 매개 변수로 지정된 섹션의 모든 키 이름이 lpReturnedString 매개 변수로 지정된 버퍼에 복사됩니다.</param>
        /// <param name="defaultValue">기본 문자열입니다. 초기화 파일에서 lpKeyName 키를 찾을 수 없는 경우 GetPrivateProfileString 은 기본 문자열을 lpReturnedString 버퍼에 복사합니다.</br>이 매개 변수가 NULL이면 기본값은 빈 문자열 ""입니다.</br>후행 빈 문자가 있는 기본 문자열을 지정하지 마세요. 함수는 lpReturnedString 버퍼에 null 문자를 삽입하여 후행 공백을 제거합니다.</param>
        /// <param name="returnedString">검색된 문자열을 수신하는 버퍼에 대한 포인터입니다.</param>
        /// <param name="size">lpReturnedString 매개 변수가 가리키는 버퍼의 크기(문자)입니다.</param>
        /// <param name="fileName">초기화 파일의 이름입니다. 이 매개 변수에 파일에 대한 전체 경로가 포함되어 있지 않으면 시스템에서 Windows 디렉터리에서 파일을 검색합니다.</param>
        /// <returns>반환 값은 종료 null 문자를 포함하지 않고 버퍼에 복사된 문자 수입니다.</returns>
        [DllImport("kernel32.dll")]
        protected static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        // 모든 Section, 모든 Key를 가져올 때는 StringBuilder를 사용하면 안되는 듯 함.
        [DllImport("kernel32")]
        protected static extern int GetPrivateProfileString(string lpAppName, int lpKeyName, string lpDefault, [MarshalAs(UnmanagedType.LPArray)] byte[] lpReturnedString, int nSize, string lpFileName);
        [DllImport("kernel32")]
        protected static extern int GetPrivateProfileString(int lpAppName, string lpKeyName, string lpDefault, [MarshalAs(UnmanagedType.LPArray)] byte[] lpReturnedString, int nSize, string lpFileName);


        /// <summary>
        /// 초기화 파일의 지정된 섹션에서 키와 연결된 정수 를 검색합니다.
        /// </summary>
        /// <param name="appName">초기화 파일에 있는 섹션의 이름입니다.</param>
        /// <param name="keyName">값을 검색할 키의 이름입니다. 이 값은 문자열 형식입니다. GetPrivateProfileInt 함수는 문자열을 정수로 변환하고 정수를 반환합니다.</param>
        /// <param name="defaultValue">초기화 파일에서 키 이름을 찾을 수 없는 경우 반환할 기본값입니다.</param>
        /// <param name="fileName">초기화 파일의 이름입니다. 이 매개 변수에 파일에 대한 전체 경로가 포함되어 있지 않으면 시스템에서 Windows 디렉터리에서 파일을 검색합니다.</param>
        /// <returns>반환 값은 지정된 초기화 파일에서 지정된 키 이름 다음에 오는 문자열에 해당하는 정수입니다. 키를 찾을 수 없는 경우 반환 값은 지정된 기본값입니다.</returns>
        [DllImport("kernel32.dll")]
        protected static extern int GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);


        /// <summary>
        /// 문자열을 초기화 파일의 지정된 섹션에 복사합니다.
        /// </summary>
        /// <param name="appName">문자열을 복사할 섹션의 이름입니다. 섹션이 없으면 만들어집니다. 섹션의 이름은 대/소문자를 구분합니다. 문자열은 대문자와 소문자의 조합일 수 있습니다.</param>
        /// <param name="keyName">문자열과 연결할 키의 이름입니다. 지정된 섹션에 키가 없으면 생성됩니다. 이 매개 변수가 NULL이면 섹션 내의 모든 항목을 포함하여 전체 섹션이 삭제됩니다.</param>
        /// <param name="value">파일에 쓸 null로 끝나는 문자열입니다. 이 매개 변수가 NULL이면 lpKeyName 매개 변수가 가리키는 키가 삭제됩니다.</param>
        /// <param name="fileName">초기화 파일의 이름입니다.</br>유니코드 문자를 사용하여 파일을 만든 경우 함수는 유니코드 문자를 파일에 씁니다. 그렇지 않으면 함수는 ANSI 문자를 씁니다.</param>
        /// <returns>함수가 문자열을 초기화 파일에 성공적으로 복사하면 반환 값은 0이 아닌 값입니다.</br>함수가 실패하거나 가장 최근에 액세스한 초기화 파일의 캐시된 버전을 플러시하는 경우 반환 값은 0입니다. 확장 오류 정보를 가져오려면 GetLastError를 호출합니다.</returns>
        [DllImport("kernel32.dll")]
        protected static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);


        /// <summary>
        /// INI파일 내 모든 Section 이름을 가져온다.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string[] GetSectionNames(string fileName)
        {
            for (int maxsize = 500; true; maxsize *= 2)
            {
                byte[] bytes = new byte[maxsize];
                int size = GetPrivateProfileString(0, "", "", bytes, maxsize, fileName);

                if (size < maxsize - 2)
                {
                    string sections = Encoding.Default.GetString(bytes, 0, size - (size > 0 ? 1 : 0));
                    return sections.Split(new char[] { '\0' });
                }
            }
        }


        /// <summary>
        /// 해당 section의 모든 키 리스트를 가져온다.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string[] GetEntryNames(string section, string fileName)
        {
            for (int maxsize = 500; true; maxsize *= 2)
            {
                byte[] bytes = new byte[maxsize];
                int size = GetPrivateProfileString(section, 0, "", bytes, maxsize, fileName);

                if (size < maxsize - 2)
                {
                    string entries = Encoding.Default.GetString(bytes, 0, size - (size > 0 ? 1 : 0));
                    return entries.Split(new char[] { '\x0' });
                }
            }
        }



        /// <summary>
        /// String값을 읽어온다.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetString(string fileName, string section, string key, string defaultValue = "")
        {
            StringBuilder sb = new StringBuilder(4096);
            GetPrivateProfileString(section, key, defaultValue, sb, sb.Capacity, fileName);
            return sb.ToString();
        }

        /// <summary>
        /// Integer값을 읽어온다.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInteger(string fileName, string section, string key, int defaultValue = 0)
        {
            return GetPrivateProfileInt(section, key, defaultValue, fileName);
        }

        /// <summary>
        /// Boolean 값을 읽어온다. "TRUE"만 true로 리턴. 나머지는 false
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool GetBoolean(string fileName, string section, string key, bool defaultValue = false)
        {
            string result = GetString(fileName, section, key, "false");
            if (result.ToUpper().Equals("TRUE"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// string 값을 저장한다.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetValue(string fileName, string section, string key, string value)
        {
            return WritePrivateProfileString(section, key, value, fileName);
        }

        /// <summary>
        /// Integer 값을 저장한다.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetValue(string fileName, string section, string key, int value)
        {
            return WritePrivateProfileString(section, key, value.ToString(), fileName);
        }

        /// <summary>
        /// Boolean 값을 저장한다.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetValue(string fileName, string section, string key, bool value)
        {
            return WritePrivateProfileString(section, key, value.ToString(), fileName);
        }


        /// <summary>
        /// 지정한 Section을 삭제
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        public static bool RemoveSection(string fileName, string section)
        {
            return WritePrivateProfileString(section, null, null, fileName);
        }

        /// <summary>
        /// 지정한 Section, Key 항목을 삭제
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        public static bool RemoveKey(string fileName, string section, string key)
        {
            return WritePrivateProfileString(section, key, null, fileName);
        }
    }
}
