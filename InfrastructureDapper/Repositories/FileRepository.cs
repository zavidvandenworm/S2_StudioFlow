using System.Data;
using Dapper;
using Domain.DTO;
using Domain.Entities;
using InfrastructureDapper.Interfaces;

namespace InfrastructureDapper.Repositories;

public class FileRepository : IFileRepository
{
    private readonly IDbConnection _dbConnection;

    public FileRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task CreateFile(CreateFileDto createFileDto)
    {
        const string newFileIdSql =
            @"INSERT INTO files (`id`, `projectId`) VALUES (null, @projectid); SELECT LAST_INSERT_ID()";

        var paramsNew = new DynamicParameters();
        paramsNew.Add("@projectid", createFileDto.ProjectId);

        var fileId = await _dbConnection.ExecuteScalarAsync<int>(newFileIdSql, paramsNew);

        const string newFileSql =
            @"INSERT INTO fileversions (id, fileId, fileContents, fileName, version, created) VALUES (null, @fileid, @filecontents, @filename, 1, null); SELECT LAST_INSERT_ID()";

        var paramsVersion = new DynamicParameters();
        paramsVersion.Add("@fileid", fileId);
        paramsVersion.Add("@filecontents", createFileDto.Content);
        paramsVersion.Add("@filename", createFileDto.FileName);

        var fileVersionId = await _dbConnection.ExecuteScalarAsync(newFileSql, paramsVersion);
    }

    public async Task<ProjectFile> GetFile(int fileId)
    {
        const string sql =
            @"SELECT * FROM fileversions WHERE fileId = @fileid AND id = (SELECT MAX(id) FROM fileversions WHERE fileId = @fileid)";
        
        var parameters = new DynamicParameters();
        parameters.Add("@fileid", fileId);
        var file = await _dbConnection.QuerySingleAsync<ProjectFile>(sql, parameters);
        return file;
    }

    public async Task<List<ProjectFile>> GetProjectFiles(int projectId)
    {
        const string sql = @"SELECT id FROM files WHERE projectId = @projectid";

        var parameters = new DynamicParameters();
        parameters.Add("@projectid", projectId);

        var fileIds = await _dbConnection.QueryAsync<int>(sql, parameters);

        List<ProjectFile> files = [];

        foreach (var id in fileIds)
        {
            files.Add(await GetFile(id));
        }

        return files;
    }

    public async Task DeleteFile(int fileId)
    {
        const string sql = @"DELETE FROM files WHERE id = @fileid; DELETE FROM fileversions WHERE fileId = @fileid";
        var parameters = new DynamicParameters();
        parameters.Add("@fileid", fileId);

        await _dbConnection.ExecuteAsync(sql, parameters);
    }
}