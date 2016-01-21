CREATE DATABASE `Friend` /*!40100 DEFAULT CHARACTER SET latin1 */;
CREATE TABLE `PriceLog` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ShopID` varchar(200) NOT NULL,
  `ProductID` varchar(200) NOT NULL,
  `SaleDate` datetime NOT NULL,
  `SalePrice` decimal(10,4) DEFAULT NULL,
  `SaleVolume` decimal(10,4) DEFAULT NULL,
  `CreatedBy` varchar(200) NOT NULL,
  `CreatedWhen` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=latin1;


