﻿namespace ShoesShop.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string objectName, Type objectType) : base($@"Entity '{objectName}' ({objectType}) not found ") { }
    }
}
