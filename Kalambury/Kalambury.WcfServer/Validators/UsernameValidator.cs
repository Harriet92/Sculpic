using System;
using System.Text.RegularExpressions;
using Kalambury.WcfServer.Models;

namespace Kalambury.WcfServer.Validators
{
    public static class UsernameValidator
    {
        public static bool IsUsernameValid(string username)
        {
            return IsLenghtValid(username) && !ContainsSpecialCharacters(username);
        }

        private static bool IsLenghtValid(string username)
        {
            return !String.IsNullOrEmpty(username) &&
                username.Length >= User.MIN_USERNAME_LEN &&
                username.Length <= User.MAX_USERNAME_LEN;
        }

        private static bool ContainsSpecialCharacters(string username)
        {
            var regexSpecialCharactersCheck = new Regex("^[a-zA-Z0-9]*$");
            return !regexSpecialCharactersCheck.IsMatch(username);
        }
    }
}
