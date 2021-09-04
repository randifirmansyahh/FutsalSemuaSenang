/*
SQLyog Ultimate v13.1.1 (32 bit)
MySQL - 10.4.17-MariaDB : Database - futsal_semua_senang
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`futsal_semua_senang` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;

USE `futsal_semua_senang`;

/*Table structure for table `booking` */

DROP TABLE IF EXISTS `booking`;

CREATE TABLE `booking` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdUser` int(11) NOT NULL,
  `NamaLapangan` text DEFAULT NULL,
  `Tanggal` text DEFAULT NULL,
  `JamMulai` text DEFAULT NULL,
  `JamSelesai` text DEFAULT NULL,
  `Harga` int(11) NOT NULL,
  `Status` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4;

/*Data for the table `booking` */

insert  into `booking`(`Id`,`IdUser`,`NamaLapangan`,`Tanggal`,`JamMulai`,`JamSelesai`,`Harga`,`Status`) values 
(1,2,'Alpha','2021-09-04','13','14',100000,1),
(2,2,'Alpha','2021-09-04','14','15',100000,1),
(3,2,'Beta','2021-09-04','14','15',100000,1),
(4,2,'Charlie','2021-09-04','13','14',100000,1),
(20,57694768,'Beta','2021-09-04','14','15',100000,1),
(21,17235697,'Charlie','2021-09-04','13','14',100000,1),
(22,71347605,'Alpha','2021-09-06','13','14',100000,1);

/*Table structure for table `roles` */

DROP TABLE IF EXISTS `roles`;

CREATE TABLE `roles` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` text DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

/*Data for the table `roles` */

insert  into `roles`(`Id`,`Name`) values 
(1,'Admin'),
(2,'User');

/*Table structure for table `user` */

DROP TABLE IF EXISTS `user`;

CREATE TABLE `user` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` text DEFAULT NULL,
  `Password` text DEFAULT NULL,
  `Email` text DEFAULT NULL,
  `Status` text DEFAULT NULL,
  `RoleId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_User_RoleId` (`RoleId`),
  CONSTRAINT `FK_User_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `roles` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

/*Data for the table `user` */

insert  into `user`(`Id`,`Name`,`Password`,`Email`,`Status`,`RoleId`) values 
(1,'Randi Firmansyah','12344321','randykelvin26@gmail.com','1',1),
(2,'Udin','1','randykelvin2626@gmail.com','1',2);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
