﻿namespace Words.Api.Services
{
    public interface IDatabaseDataMigrator
    {
        Task DoWork();
    }
}