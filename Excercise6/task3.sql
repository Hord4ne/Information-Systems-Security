--As the differential backups increase in size, restoring a differential backup
--can significantly increase the time that is required to restore a database.
--Therefore, we recommend that you take a new full backup at set intervals to
--establish a new differential base for the data.
--For example, you might take a weekly full backup of the whole database (that is, a full database backup) 
--followed by a regular series of differential database backups during the week.

-- At restore time, before you restore a differential backup, you must restore its base.
-- Then, restore only the most recent differential backup to bring the database forward to the time when that differential backup was created.
-- Typically, you would restore the most recent full backup followed by the most recent differential backup that is based on that full backup.

--fullback
BACKUP DATABASE [TEST-ISS]
TO
          DISK = 'D:\backup\TESTISS.Bak'
          WITH FORMAT, --Specifies whether the   media header should be written on the volumes used for this backup operation,   overwriting any existing media header and backup sets.
          COMPRESSION, --In SQL Server 2008 Enterprise edition and later   versions only, specifies whether compression is performed on this backup,   overriding the server-level default.
          MEDIANAME = 'TESTISSBackups',
          NAME   = 'Full database   backup of TESTISS';
GO


--diff
BACKUP DATABASE [TEST-ISS]
TO
          DISK = 'D:\backup\TESTISSDifferential.Bak'
          WITH FORMAT, --Specifies whether the   media header should be written on the volumes used for this backup operation,   overwriting any existing media header and backup sets.
          COMPRESSION, --In SQL Server 2008 Enterprise edition and later   versions only, specifies whether compression is performed on this backup,   overriding the server-level default.
          DIFFERENTIAL, --Specifies that backup should consist only of the   portions of the database changed since the last full backup
          MEDIANAME = 'TESTISSBackups',
          NAME   = 'Differential   database backup of TESTISS';
GO
