using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogiPark.MVVM.Model
{
    public class UserDataManager
    {
        /************* UserLoginData *************/
        [ProtoContract]
        public class LoginData
        {
            [ProtoMember(1)]
            public string username;
            [ProtoMember(2)]
            public string password;

            public string GetUserName()
            {
                return this.username;
            }

            public string GetPassword()
            {
                return this.password;
            }

            public void SetUserName(string username)
            {
                this.username = username;
            }

            public void SetPassword(string password)
            {
                this.password = password;
            }

            public byte[] SerializeToByteArray()
            {
                using (var stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, this);
                    return stream.ToArray();
                }
            }

            public LoginData deserializeLoginData(byte[] buffer)
            {
                using (var memStream = new MemoryStream(buffer))
                {
                    return Serializer.Deserialize<LoginData>(memStream);
                }
            }
        }

        /************* Login *************/
        public class Login
        {
            private LoginData loginData;

            public Login(LoginData data)
            {
                loginData = data;
            }

            public LoginData GetUserData()
            {
                return loginData;
            }

            public void SetUserData(string username, string password)
            {
                loginData.username = username;
                loginData.password = password;
            }

            public string LoginUser(string filePath)
            {
                string message = "Username or password is incorrect";
                try
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string line1 = reader.ReadLine();
                        string line2 = reader.ReadLine();

                        while ((line1 != null) && (line2 != null))
                        {
                            if ((line1 == loginData.GetUserName()) && (line2 == loginData.GetPassword()))
                            {

                                message = "User signed in";
                                break;
                            }

                            line1 = reader.ReadLine();
                            line2 = reader.ReadLine();
                        }



                        reader.Close();
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message + "Error Signing in user");
                }

                return message;
            }
        }
    }
}