using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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


        /***************** Sign Up *****************/

       
        // check if username already exists

        public class SignUp
        {
            public SignUpData signUpData;
            public SignUp(SignUpData data)
            {
                signUpData = data;
            }


            public SignUpData GetSignUpData()
            {
                return SignUpData;
            }

            public void SetSignUpData(string username, string password)
            {
                SignUpData.username = username;
                SignUpData.password = password;
            }
            // 
            // check if username already exists
            // if (SignUpData.username == loginData.username) 
            // {
            // string signUpMessage = "Username already exists please try another again!!! /o\\ ";
            // }

            public string SignUpUser(string filename)
            {
                string signUpMessage = "Please enter username to register!!!! \\o/";
                try
                {
                    using (StreamReader streamReader = new StreamReader(filename))
                    {
                        string line1 = streamReader.ReadLine();
                        // string line2 = streamReader.ReadLine();

                        while ((line1 != null)) //  && (line2 != null))
                        {
                            if ((line1 == signUpData.GetUserName()) //  && (line2 == signUpData.GetPassword()))
                            {

                                signUpMessage = "Username already exists please try another!!! /o\\";
                                break;
                            }

                            line1 = streamReader.ReadLine();
                            // line2 = streamReader.ReadLine();
                        }



                        streamReader.Close();


                        // another method to write username and password and append to userDB.txt file 
                        // Append text to an existing file named "WriteLines.txt".
                        // helper function should return string back if signup was successful to signUpUser()

                        // public string SignUp(string filename)
                        using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "userDB.txt"), true))
                        {
                            outputFile.WriteLine(signUpData.GetUserName()); // will add the username to the text file
                            outputFile.WriteLine(signUpData.GetUserPassword()); //will add the password to the text file
                            outputFile.Close();
                            // Console.WriteLine("Success registering user!!! \\o/"); // return success


                        }
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message + "Error registering user!!! /o\\");
                }

                return signUpMessage;
            }
        }
    }
}