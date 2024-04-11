USE studioflow;
SELECT * from projects WHERE id in (select projectId from projectmembers where userId = @id)