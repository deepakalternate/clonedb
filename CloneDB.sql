CREATE DATABASE `clonedb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */;

CREATE TABLE `people` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `DOB` date DEFAULT NULL,
  `Sex` int(11) DEFAULT NULL,
  `Bio` varchar(2000) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `movies` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `ReleaseDate` date DEFAULT NULL,
  `Plot` varchar(2000) DEFAULT NULL,
  `PosterPath` varchar(1000) DEFAULT NULL,
  `ProducerId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `actormoviemapping` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `movieid` int(11) DEFAULT NULL,
  `actorid` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `addmovie`(IN par_name VARCHAR(200),
IN par_releasedate DATE,
IN par_plot VARCHAR(2000),
IN par_poster VARCHAR(1000),
IN par_producerid INT(11)
)
BEGIN
INSERT INTO `clonedb`.`movies`
(`Name`,
`ReleaseDate`,
`Plot`,
`PosterPath`,
`ProducerId`)
VALUES
(
par_name,
par_releasedate,
par_plot,
par_poster,
par_producerid);

select last_insert_id() as id;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `addperson`(
IN par_name varchar(200),
IN par_dob date,
IN par_sex int,
IN par_bio varchar(2000))
BEGIN
INSERT INTO `clonedb`.`people`
(`Name`,
`DOB`,
`Sex`,
`Bio`)
VALUES
(par_name,
par_dob,
par_sex,
par_bio);

END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `getallmovies`()
BEGIN
SELECT 
    m.Id AS MovieId,
    m.Name AS MovieName,
    m.ReleaseDate AS ReleaseDate,
    m.Plot AS Plot,
    m.PosterPath AS PosterPath,
    m.ProducerId AS ProducerId,
    p.Name AS ProducerName,
    p.DOB AS ProducerDOB,
    p.Sex AS ProducerSex,
    p.Bio AS ProducerBio
FROM
    movies AS m
        INNER JOIN
    people AS p ON m.ProducerId = p.Id;
    
SELECT 
    amm.movieid AS MovieId,
    amm.actorid AS ActorId,
    p.Name AS ActorName,
    p.DOB AS ActorDOB,
    p.Sex AS ActorSex,
    p.Bio AS ActorBio
FROM
    actormoviemapping AS amm
        INNER JOIN
    people AS p ON amm.actorid = p.Id;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `getmoviedatabyid`(IN par_movieid int(11))
BEGIN
SELECT 
    m.Id AS MovieId,
    m.Name AS MovieName,
    m.ReleaseDate AS ReleaseDate,
    m.Plot AS Plot,
    m.PosterPath AS PosterPath,
    m.ProducerId AS ProducerId,
    p.Name AS ProducerName,
    p.DOB AS ProducerDOB,
    p.Sex AS ProducerSex,
    p.Bio AS ProducerBio
FROM
    movies AS m
        INNER JOIN
    people AS p ON m.ProducerId = p.Id
WHERE
    m.Id = par_movieid;
    
SELECT 
    amm.movieid AS MovieId,
    amm.actorid AS ActorId,
    p.Name AS ActorName,
    p.DOB AS ActorDOB,
    p.Sex AS ActorSex,
    p.Bio AS ActorBio
FROM
    actormoviemapping AS amm
        INNER JOIN
    people AS p ON amm.actorid = p.Id
WHERE
    amm.movieid = par_movieid;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `removeactormoviemappings`(IN par_movieid int(11))
BEGIN
DELETE FROM actormoviemapping
WHERE movieid = par_movieid;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `saveactortomovie`(IN par_movieid int(11),
IN par_actorid int(11))
BEGIN

INSERT INTO `clonedb`.`actormoviemapping`
(`movieid`,
`actorid`)
VALUES
(par_movieid,
par_actorid);

END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `updatemovie`(IN par_name VARCHAR(200),
IN par_movieid int(11),
IN par_releasedate DATE,
IN par_plot VARCHAR(2000),
IN par_poster VARCHAR(1000),
IN par_producerid INT(11)
)
BEGIN

UPDATE movies
SET
Name = par_name,
ReleaseDate = par_releasedate,
Plot = par_plot,
PosterPath = par_poster,
ProducerId = par_producerid
WHERE Id = par_movieid;

END$$
DELIMITER ;


