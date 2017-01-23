using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using DatabaseAccess;
using Ninject;


namespace Asterisk.AccountManagement
{
  public class AsteriskMembershipProvider : MembershipProvider
  {

    //todo: fix all of NotImplementedException -- what are you going to do with them?  what needs implementing?

    public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion,
                                              string passwordAnswer, bool isApproved, object providerUserKey,
                                              out MembershipCreateStatus status)
    {
      throw new NotImplementedException();
    }

    public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion,
                                                         string newPasswordAnswer)
    {
      throw new NotImplementedException();
    }

    public override string GetPassword(string username, string answer)
    {
      throw new NotImplementedException();
    }

    public override bool ChangePassword(string username, string oldPassword, string newPassword)
    {
      throw new NotImplementedException();
    }

    public override string ResetPassword(string username, string answer)
    {
      throw new NotImplementedException();
    }

    public override void UpdateUser(MembershipUser user)
    {
      throw new NotImplementedException();
    }

    public override bool ValidateUser(string username, string password)
    {
      //TODO: get rid of this with structuremap
      var x = new Repository().GetFromName<IUserConfig>(username);
      var y = x.Password;
      return y == password;

      // return Repository.GetFromName<IUserConfig>(username).Password == password;
    }

    public override bool UnlockUser(string userName)
    {
      throw new NotImplementedException();
    }

    public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
    {
      throw new NotImplementedException();
    }

    public override MembershipUser GetUser(string username, bool userIsOnline)
    {
      throw new NotImplementedException();
    }

    public override string GetUserNameByEmail(string email)
    {
      throw new NotImplementedException();
    }

    public override bool DeleteUser(string username, bool deleteAllRelatedData)
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

    public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
                                                             out int totalRecords)
    {
      throw new NotImplementedException();
    }

    public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
                                                              out int totalRecords)
    {
      throw new NotImplementedException();
    }

    public override bool EnablePasswordRetrieval
    {
      get { throw new NotImplementedException(); }
    }

    public override bool EnablePasswordReset
    {
      get { throw new NotImplementedException(); }
    }

    public override bool RequiresQuestionAndAnswer
    {
      get { throw new NotImplementedException(); }
    }

    public override string ApplicationName
    {
      get { throw new NotImplementedException(); }
      set { throw new NotImplementedException(); }
    }

    public override int MaxInvalidPasswordAttempts
    {
      get { throw new NotImplementedException(); }
    }

    public override int PasswordAttemptWindow
    {
      get { throw new NotImplementedException(); }
    }


    public override bool RequiresUniqueEmail
    {
      get { return false; }
    }

    public override MembershipPasswordFormat PasswordFormat
    {
      get { throw new NotImplementedException(); }
    }


    public override int MinRequiredPasswordLength
    {
      get { return 4; }
    }

    public override int MinRequiredNonAlphanumericCharacters
    {
      get { throw new NotImplementedException(); }
    }

    public override string PasswordStrengthRegularExpression
    {
      get { throw new NotImplementedException(); }
    }
  }
}