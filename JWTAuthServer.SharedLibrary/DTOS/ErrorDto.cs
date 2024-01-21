using System;
using System.Collections.Generic;

namespace JWTAuthServer.SharedLibrary.DTOS
{
    public class ErrorDto
    {
        public List<String> Errors { get; set; }
        public bool IsShow { get; set; }

        public ErrorDto() 
        {
            Errors = new List<String>();
        }
        public ErrorDto(string error,bool isShow)
        {
            Errors.Add(error);
            isShow = true;
        }
        public ErrorDto(List<string> errors, bool isShow)
        {
            Errors = errors;
            IsShow = isShow;
        }
    }
}
