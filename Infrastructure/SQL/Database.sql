CREATE DATABASE IF NOT EXISTS StudioFlow;
USE StudioFlow;

CREATE TABLE `Users` (
  `id` int PRIMARY KEY,
  `username` varchar(255) UNIQUE,
  `email` varchar(255),
  `passwordHash` varchar(255)
);

CREATE TABLE `Profiles` (
  `userId` int,
  `displayName` varchar(255) UNIQUE
);

CREATE TABLE `Tasks` (
  `id` int PRIMARY KEY,
  `name` varchar(255),
  `description` varchar(255),
  `deadline` timestamp
);

CREATE TABLE `TaskMembers` (
  `id` int PRIMARY KEY,
  `taskId` int,
  `userId` int
);

CREATE TABLE `TaskTags` (
  `id` int PRIMARY KEY,
  `taskId` int,
  `name` varchar(255)
);

CREATE TABLE `TaskFiles` (
  `id` int PRIMARY KEY,
  `taskId` int,
  `fileId` int
);

CREATE TABLE `Files` (
  `id` int PRIMARY KEY
);

CREATE TABLE `FileVersions` (
  `id` int PRIMARY KEY,
  `fileId` int,
  `fileContents` blob,
  `fileName` varchar(255),
  `version` int,
  `created` timestamp
);

CREATE TABLE `Messages` (
  `id` int PRIMARY KEY,
  `userId` int,
  `projectId` int,
  `content` varchar(255),
  `created` timestamp
);

CREATE TABLE `MessageAttachments` (
  `id` int PRIMARY KEY,
  `messageId` int,
  `fileId` int
);

CREATE TABLE `Projects` (
  `id` int PRIMARY KEY,
  `name` varchar(255),
  `description` varchar(255),
  `daw` int
);

CREATE TABLE `ProjectFiles` (
  `id` int PRIMARY KEY,
  `projectId` int,
  `fileId` int
);

CREATE TABLE `ProjectTags` (
  `id` int PRIMARY KEY,
  `projectId` int,
  `name` varchar(255)
);

CREATE TABLE `ProjectMembers` (
  `id` int PRIMARY KEY,
  `userId` int,
  `projectId` int,
  `role` int
);

ALTER TABLE `Profiles` ADD FOREIGN KEY (`userId`) REFERENCES `Users` (`id`);

ALTER TABLE `TaskMembers` ADD FOREIGN KEY (`taskId`) REFERENCES `Tasks` (`id`);

ALTER TABLE `TaskMembers` ADD FOREIGN KEY (`userId`) REFERENCES `Users` (`id`);

ALTER TABLE `TaskTags` ADD FOREIGN KEY (`taskId`) REFERENCES `Tasks` (`id`);

ALTER TABLE `TaskFiles` ADD FOREIGN KEY (`taskId`) REFERENCES `Tasks` (`id`);

ALTER TABLE `TaskFiles` ADD FOREIGN KEY (`fileId`) REFERENCES `Files` (`id`);

ALTER TABLE `FileVersions` ADD FOREIGN KEY (`fileId`) REFERENCES `Files` (`id`);

ALTER TABLE `Messages` ADD FOREIGN KEY (`userId`) REFERENCES `Users` (`id`);

ALTER TABLE `Messages` ADD FOREIGN KEY (`projectId`) REFERENCES `Projects` (`id`);

ALTER TABLE `MessageAttachments` ADD FOREIGN KEY (`messageId`) REFERENCES `Messages` (`id`);

ALTER TABLE `MessageAttachments` ADD FOREIGN KEY (`fileId`) REFERENCES `Files` (`id`);

ALTER TABLE `ProjectFiles` ADD FOREIGN KEY (`projectId`) REFERENCES `Projects` (`id`);

ALTER TABLE `ProjectFiles` ADD FOREIGN KEY (`fileId`) REFERENCES `Files` (`id`);

ALTER TABLE `ProjectTags` ADD FOREIGN KEY (`projectId`) REFERENCES `Projects` (`id`);

ALTER TABLE `ProjectMembers` ADD FOREIGN KEY (`userId`) REFERENCES `Users` (`id`);

ALTER TABLE `ProjectMembers` ADD FOREIGN KEY (`projectId`) REFERENCES `Projects` (`id`);
