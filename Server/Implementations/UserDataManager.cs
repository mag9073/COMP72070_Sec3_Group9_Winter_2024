using ProtoBuf;
using Server.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Implementations
{
    public class UserDataManager
    {
        public string PerformLogin(UserDataManager.LoginData loginData)
        {
            Console.WriteLine($"Username: {loginData.GetUserName()}");
            Console.WriteLine($"Password: {loginData.GetPassword()}");

            Login login = new Login(loginData);
            return login.LoginUser(Constants.UserDB_FilePath);
        }

        public string PerformAdminLogin(UserDataManager.LoginData loginData)
        {
            Console.WriteLine($"Username: {loginData.GetUserName()}");
            Console.WriteLine($"Password: {loginData.GetPassword()}");

            Login login = new Login(loginData);
            return login.LoginUser(Constants.AdminDB_FilePath);
        }

        public string PerformSignUp(UserDataManager.SignUpData signUpData)
        {
            Console.WriteLine($"Username:  {signUpData.GetUserName()}");
            Console.WriteLine($"Password:  {signUpData.GetPassword()}");

            SignUp signUp = new SignUp(signUpData);
            return signUp.SignUpUser(Constants.UserDB_FilePath);
        }

        [ProtoContract]
        public class LoginData
        {
            [ProtoMember(1)]
            public string username = String.Empty;
            [ProtoMember(2)]
            public string password = String.Empty;

            public string GetUserName()
            {
                return this.username;
            }

            public string GetPassword()
            {
                return this.password;
            }

            //public void SetUserName(string username)
            //{
            //    this.username = username;
            //}

            //public void SetPassword(string password)
            //{
            //    this.password = password;
            //}

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

            public Login(UserDataManager.LoginData data)
            {
                loginData = data;
            }

            //public LoginData GetUserData()
            //{
            //    return loginData;
            //}

            //public void SetUserData(string username, string password)
            //{
            //    loginData.username = username;
            //    loginData.password = password;
            //}

            public string LoginUser(string filename)
            {
                string loginMessage = "Username or Password is incorrect. Please try again!!! /o\\";
                try
                {
                    using (StreamReader streamReader = new StreamReader(filename))
                    {
                        string line1 = streamReader.ReadLine();
                        string line2 = streamReader.ReadLine();

                        while ((line1 != null) && (line2 != null))
                        {
                            if ((line1 == loginData.GetUserName()) && (line2 == loginData.GetPassword()))
                            {

                                loginMessage = "Username and password are Correct!!! \\o/";
                                break;
                            }

                            line1 = streamReader.ReadLine();
                            line2 = streamReader.ReadLine();
                        }



                        streamReader.Close();
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message + "Error Signing in user");
                }

                return loginMessage;
            }
        }

        /***************** Sign Up *****************/

        /************* UserSignUpData *************/
        [ProtoContract]
        public class SignUpData
        {
            [ProtoMember(1)]
            public string username = String.Empty;
            [ProtoMember(2)]
            public string password = String.Empty;

            public string GetUserName()
            {
                return this.username;
            }

            public string GetPassword()
            {
                return this.password;
            }

            //public void SetUserName(string username)
            //{
            //    this.username = username;
            //}

            //public void SetPassword(string password)
            //{
            //    this.password = password;
            //}

            public byte[] SerializeToByteArray()
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, this);
                    return stream.ToArray();
                }
            }

            public SignUpData deserializeSignUpData(byte[] buffer)
            {
                using (MemoryStream memStream = new MemoryStream(buffer))
                {
                    return Serializer.Deserialize<SignUpData>(memStream);
                }
            }
        }


        // check if username already exists

        public class SignUp
        {
            public SignUpData signUpData;
            public SignUp(SignUpData data)
            {
                this.signUpData = data;
            }


            //public SignUpData GetSignUpData()
            //{
            //    return this.signUpData;
            //}

            //public void SetSignUpData(string username, string password)
            //{
            //    this.signUpData.username = username;
            //    this.signUpData.password = password;
            //}
            // 
            // check if username already exists
            // if (SignUpData.username == loginData.username) 
            // {
            // string signUpMessage = "Username already exists please try another again!!! /o\\ ";
            // }

            public string SignUpUser(string filePath)
            {
                string signUpMessage = "Successfully Sign Up \\o/";
                bool userNameExists = false;
                try
                {
                    using (StreamReader streamReader = new StreamReader(filePath))
                    {
                        string line1 = streamReader.ReadLine();
                        string line2 = streamReader.ReadLine();

                        while ((line1 != null)) //  && (line2 != null))
                        {
                            if ((line1 == signUpData.GetUserName())) //  && (line2 == signUpData.GetPassword()))
                            {

                                signUpMessage = "Username already exists please try another!!! /o\\";
                                userNameExists = true;
                                break;
                            }

                            line1 = streamReader.ReadLine();
                            // line2 = streamReader.ReadLine();
                        }



                        streamReader.Close();


                        // another method to write username and password and append to userDB.txt file 
                        // Append text to an existing file named "userDB.txt".
                        // helper function should return string back if signup was successful to signUpUser()

                        // public string SignUp(string filename)
                        if (!userNameExists)
                        {
                            using (StreamWriter outputFile = new StreamWriter(filePath, true))
                            {
                                outputFile.WriteLine();
                                outputFile.WriteLine(signUpData.GetUserName()); // will add the username to the text file
                                outputFile.Write(signUpData.GetPassword()); //will add the password to the text file
                                outputFile.Close();
                                // Console.WriteLine("Success registering user!!! \\o/"); // return success
                            }

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
