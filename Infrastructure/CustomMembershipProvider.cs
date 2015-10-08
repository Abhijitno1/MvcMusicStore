using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using MvcMusicStore.Models;
using System.Security.Cryptography;
using System.Text;

namespace MvcMusicStore.Infrastructure
{
    public class CustomMembershipProvider: MembershipProvider
    {
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var validatePwdEventArgs = new ValidatePasswordEventArgs(username, oldPassword, true);
            OnValidatingPassword(validatePwdEventArgs);
            if (validatePwdEventArgs.Cancel) //If user has cancelled the operation then
                return false;
            using (var repo = new UsersRepository())
            {
                oldPassword = GetMD5Hash(oldPassword);
                var foundUser = repo.GetUser(username, oldPassword);
                if (foundUser == null)
                    return false;
                newPassword = GetMD5Hash(newPassword);
                foundUser.password = newPassword;
                repo.UpdateUser(foundUser);
            }
            return true;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            var validatePwdEventArgs = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(validatePwdEventArgs);
            if (validatePwdEventArgs.Cancel) //If user has cancelled the operation then
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && !string.IsNullOrEmpty(GetUserNameByEmail(email)))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            if (GetUser(username, true) != null)
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            var newUser = new User();
            newUser.UserName = username;
            newUser.password = password;
            newUser.UserEmail = email;
            MembershipUser registeredUser;
            try
            {
                using (var repo = new UsersRepository())
                {
                    repo.RegisterUser(newUser);
                }
                registeredUser = GetUser(username, true);
            }
            catch
            {
                status = MembershipCreateStatus.UserRejected;
                return null;
            }
            status = MembershipCreateStatus.Success;
            return registeredUser;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method retrieves user info from database based on user name
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            MembershipUser user = null;
            using (var repo = new UsersRepository())
            {
                var foundUser = repo.GetUser(username);
                if (foundUser != null)
                {
                    user = new MembershipUser("CustomMembershipProvider",
                        name: foundUser.UserName, providerUserKey: foundUser.UserID,
                        email: foundUser.UserEmail, passwordQuestion: string.Empty,
                        comment: string.Empty, isApproved: true, isLockedOut: false,
                        creationDate: DateTime.Now, lastLoginDate: DateTime.MinValue,
                        lastActivityDate: DateTime.MinValue, lastPasswordChangedDate: DateTime.MinValue,
                        lastLockoutDate: DateTime.MinValue
                        );
                }
            }
            return user;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Specify max number of allowed attempts if user provides invalid password
        /// </summary>
        public override int MaxInvalidPasswordAttempts
        {
            get { return 5; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Specifies whether user is required to have unique email id
        /// </summary>
        public override bool RequiresUniqueEmail
        {
            get { return false; }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            var encryptedPassword = GetMD5Hash(password);

            using (var repo = new UsersRepository())
            {
                var foundUser = repo.GetUser(username, encryptedPassword);
                if (foundUser != null)
                {
                    return true;
                }
            }
            return false;
        }

        private string GetMD5Hash(string password)
        {
            MD5 encryptor = MD5.Create();
            byte[] buffer = Encoding.Default.GetBytes(password);
            var outputBuffer = encryptor.ComputeHash(buffer);                        
            StringBuilder sb= new StringBuilder();
            foreach (byte elm in outputBuffer)
            {
                sb.Append(elm.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}