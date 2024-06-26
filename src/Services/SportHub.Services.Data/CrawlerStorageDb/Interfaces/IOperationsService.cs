﻿namespace SportHub.Services.Data.CrawlerStorageDb.Interfaces;

public interface IOperationsService
{
    Task AddOperationAsync(string operationName);

    bool IsOperationTableFull();
}