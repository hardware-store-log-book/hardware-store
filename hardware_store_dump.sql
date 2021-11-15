-- MySQL dump 10.13  Distrib 8.0.23, for Win64 (x86_64)
--
-- Host: localhost    Database: hardware_store
-- ------------------------------------------------------
-- Server version	8.0.23

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `address_delivery`
--

DROP TABLE IF EXISTS `address_delivery`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `address_delivery` (
  `id_address_delivery` int NOT NULL,
  `address` varchar(250) NOT NULL,
  PRIMARY KEY (`id_address_delivery`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `address_delivery`
--

LOCK TABLES `address_delivery` WRITE;
/*!40000 ALTER TABLE `address_delivery` DISABLE KEYS */;
INSERT INTO `address_delivery` VALUES (1,'ул. Мирослава 10А'),(2,'ул. Генерала Горбатого 4'),(3,'ул. Открытий 27'),(4,'ул. Проспект Октября 16');
/*!40000 ALTER TABLE `address_delivery` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `documents_purchases_invoices`
--

DROP TABLE IF EXISTS `documents_purchases_invoices`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `documents_purchases_invoices` (
  `id_documents_purchases_invoices` int NOT NULL,
  `sender` int NOT NULL,
  `recipient` int NOT NULL,
  `address_delivery` int NOT NULL,
  `date_registration` date NOT NULL,
  `date_accept_product` date NOT NULL,
  `total_money` decimal(10,2) NOT NULL,
  PRIMARY KEY (`id_documents_purchases_invoices`),
  KEY `idk_idx` (`address_delivery`),
  KEY `idk2_idx` (`sender`),
  KEY `idk3_idx` (`recipient`),
  CONSTRAINT `idk` FOREIGN KEY (`address_delivery`) REFERENCES `address_delivery` (`id_address_delivery`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `idk2` FOREIGN KEY (`sender`) REFERENCES `store_manager` (`id_store_manager`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `idk3` FOREIGN KEY (`recipient`) REFERENCES `warehouse_manager` (`id_warehouse_manager`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `documents_purchases_invoices`
--

LOCK TABLES `documents_purchases_invoices` WRITE;
/*!40000 ALTER TABLE `documents_purchases_invoices` DISABLE KEYS */;
INSERT INTO `documents_purchases_invoices` VALUES (1,1,1,1,'2021-11-13','2021-11-13',98942.00),(2,1,1,1,'2021-11-13','2021-11-13',17998.00),(3,1,1,1,'2021-11-13','2021-11-13',13599.00);
/*!40000 ALTER TABLE `documents_purchases_invoices` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `manufactures`
--

DROP TABLE IF EXISTS `manufactures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `manufactures` (
  `ID` int NOT NULL,
  `Name` varchar(150) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `manufactures`
--

LOCK TABLES `manufactures` WRITE;
/*!40000 ALTER TABLE `manufactures` DISABLE KEYS */;
INSERT INTO `manufactures` VALUES (1,'IEK'),(2,'Legrand'),(3,'SIEMENS'),(4,'OSRAM'),(5,'Nexans'),(6,'Rittal');
/*!40000 ALTER TABLE `manufactures` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `order`
--

DROP TABLE IF EXISTS `order`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `order` (
  `id_order` int NOT NULL,
  `address_delivery` int NOT NULL,
  `date_order` date NOT NULL,
  `status` varchar(45) NOT NULL,
  PRIMARY KEY (`id_order`),
  KEY `order_ibfk_1` (`address_delivery`),
  CONSTRAINT `order_ibfk_1` FOREIGN KEY (`address_delivery`) REFERENCES `address_delivery` (`id_address_delivery`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order`
--

LOCK TABLES `order` WRITE;
/*!40000 ALTER TABLE `order` DISABLE KEYS */;
INSERT INTO `order` VALUES (1,1,'2021-11-13','одобрен'),(2,1,'2021-11-13','одобрен'),(3,1,'2021-11-13','одобрен');
/*!40000 ALTER TABLE `order` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `order_content`
--

DROP TABLE IF EXISTS `order_content`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `order_content` (
  `ID` int NOT NULL,
  `id_order` int NOT NULL,
  `Article` varchar(8) NOT NULL,
  `Count` int NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `order_content_ibfk_1` (`id_order`),
  CONSTRAINT `order_content_ibfk_1` FOREIGN KEY (`id_order`) REFERENCES `order` (`id_order`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order_content`
--

LOCK TABLES `order_content` WRITE;
/*!40000 ALTER TABLE `order_content` DISABLE KEYS */;
INSERT INTO `order_content` VALUES (1,1,'6O551TN7',2),(2,1,'3YOXJFOK',1),(3,1,'H05WUXSJ',15),(4,2,'JLMZJD4N',1),(5,2,'5J3QEHDT',1),(6,3,'6O551TN7',1);
/*!40000 ALTER TABLE `order_content` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product`
--

DROP TABLE IF EXISTS `product`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product` (
  `article` varchar(8) NOT NULL,
  `name_product` varchar(100) NOT NULL,
  `manufacturer` int NOT NULL,
  `price` decimal(10,2) NOT NULL,
  `additional_info` longtext NOT NULL,
  PRIMARY KEY (`article`),
  KEY `a_idx` (`manufacturer`),
  CONSTRAINT `a` FOREIGN KEY (`manufacturer`) REFERENCES `manufactures` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product`
--

LOCK TABLES `product` WRITE;
/*!40000 ALTER TABLE `product` DISABLE KEYS */;
INSERT INTO `product` VALUES ('3YOXJFOK','Телевизор Xiaomi Mi TV P1 43 LED',5,35759.00,''),('4J3QEHDT','Фен POLARIS PHD 2018Ti',1,7999.00,''),('5J3QEHDT','Фен POLARIS PHD 2018Ti',1,7999.00,''),('6O551TN7','Триммер PHILIPS BT5502',3,13599.00,''),('H05WUXSJ','Клавиатура OKLICK 717G BLACK DEATH, USB',2,2399.00,''),('JLMZJD4N','Микатермический обогреватель Polaris PMH 2120 WIFI',6,9999.00,''),('RHYR0U9O','Телевизор Prestigio PTV40SN04YCISBK',4,29999.00,'');
/*!40000 ALTER TABLE `product` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `report_log`
--

DROP TABLE IF EXISTS `report_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `report_log` (
  `id_report_log` int NOT NULL,
  `id_order` int NOT NULL,
  `date_delivery` date NOT NULL,
  PRIMARY KEY (`id_report_log`),
  KEY `idkf_idx` (`id_order`),
  CONSTRAINT `idkf` FOREIGN KEY (`id_order`) REFERENCES `order` (`id_order`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `report_log`
--

LOCK TABLES `report_log` WRITE;
/*!40000 ALTER TABLE `report_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `report_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `store_manager`
--

DROP TABLE IF EXISTS `store_manager`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `store_manager` (
  `id_store_manager` int NOT NULL,
  `FIO` varchar(150) NOT NULL,
  `phone_number` varchar(12) NOT NULL,
  `gender` varchar(1) NOT NULL,
  `date_of_birth` date NOT NULL,
  `login` varchar(30) NOT NULL,
  `password` varchar(30) NOT NULL,
  `id_address` int NOT NULL,
  PRIMARY KEY (`id_store_manager`),
  KEY `store_manager_ibfk_1` (`id_address`),
  CONSTRAINT `store_manager_ibfk_1` FOREIGN KEY (`id_address`) REFERENCES `address_delivery` (`id_address_delivery`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `store_manager`
--

LOCK TABLES `store_manager` WRITE;
/*!40000 ALTER TABLE `store_manager` DISABLE KEYS */;
INSERT INTO `store_manager` VALUES (1,'Устинов Михаил Иринеевич','+79346658134','м','1993-10-10','Ustinov','us123',1),(2,'Тимофеев Антон Леонидович','+79346543412','м','1984-05-03','Timofeev','us123',2),(3,'Морозова Эстелла Лукьяновна','+79011324565','ж','1987-10-11','Morozova','mo123',3),(4,'Кузьмина Юлиана Тарасовна','+79011324565','ж','1987-10-11','Kuzmina','ku123',3);
/*!40000 ALTER TABLE `store_manager` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `warehouse_manager`
--

DROP TABLE IF EXISTS `warehouse_manager`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `warehouse_manager` (
  `id_warehouse_manager` int NOT NULL,
  `FIO` varchar(150) NOT NULL,
  `Login` varchar(30) NOT NULL,
  `Pass` varchar(30) NOT NULL,
  PRIMARY KEY (`id_warehouse_manager`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `warehouse_manager`
--

LOCK TABLES `warehouse_manager` WRITE;
/*!40000 ALTER TABLE `warehouse_manager` DISABLE KEYS */;
INSERT INTO `warehouse_manager` VALUES (1,'Белов Антон Анатольевич','Belov','123be');
/*!40000 ALTER TABLE `warehouse_manager` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-11-15 20:17:22
