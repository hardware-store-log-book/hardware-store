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
  `date_registration` datetime NOT NULL,
  `number_purchases_invoices` int NOT NULL,
  `date_accept_product` datetime NOT NULL,
  `address_delivery` varchar(250) NOT NULL,
  `FIO_sender` varchar(250) NOT NULL,
  `FIO_recipient` varchar(250) NOT NULL,
  `total_money` decimal(10,2) NOT NULL,
  PRIMARY KEY (`id_documents_purchases_invoices`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `documents_purchases_invoices`
--

LOCK TABLES `documents_purchases_invoices` WRITE;
/*!40000 ALTER TABLE `documents_purchases_invoices` DISABLE KEYS */;
/*!40000 ALTER TABLE `documents_purchases_invoices` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `order`
--

DROP TABLE IF EXISTS `order`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `order` (
  `id_order` int NOT NULL,
  `name_product` varchar(100) NOT NULL,
  `manufacturer_product` varchar(100) NOT NULL,
  `count_product` varchar(45) NOT NULL,
  `address_delivery` varchar(250) NOT NULL,
  `ordercol` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id_order`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order`
--

LOCK TABLES `order` WRITE;
/*!40000 ALTER TABLE `order` DISABLE KEYS */;
/*!40000 ALTER TABLE `order` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product`
--

DROP TABLE IF EXISTS `product`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product` (
  `id_product` int NOT NULL,
  `name_product` varchar(100) NOT NULL,
  `manufacturer` varchar(150) NOT NULL,
  `model` varchar(100) NOT NULL,
  `count_product` int NOT NULL,
  `price_for_one` decimal(10,2) NOT NULL,
  `total_price` decimal(10,2) NOT NULL,
  `additional_info` longtext,
  PRIMARY KEY (`id_product`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product`
--

LOCK TABLES `product` WRITE;
/*!40000 ALTER TABLE `product` DISABLE KEYS */;
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
  `name_product` varchar(100) NOT NULL,
  `count_product` int NOT NULL,
  `manufacturer` varchar(150) NOT NULL,
  `sender` varchar(150) NOT NULL,
  `recipient` varchar(150) NOT NULL,
  `price_for_one` decimal(10,2) NOT NULL,
  `total_price` decimal(10,2) NOT NULL,
  `doc_purchases_invoices` int NOT NULL,
  `number_order` int NOT NULL,
  PRIMARY KEY (`id_report_log`),
  KEY `doc_purchases_idx` (`doc_purchases_invoices`),
  KEY `number_order_FK_idx` (`number_order`),
  CONSTRAINT `doc_purchases_FK` FOREIGN KEY (`doc_purchases_invoices`) REFERENCES `documents_purchases_invoices` (`id_documents_purchases_invoices`),
  CONSTRAINT `number_order_FK` FOREIGN KEY (`number_order`) REFERENCES `order` (`id_order`)
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
  `start_date` datetime NOT NULL,
  `end_date` datetime NOT NULL,
  `phone_number` varchar(12) NOT NULL,
  `gender` varchar(1) NOT NULL DEFAULT 'м',
  `date_of_birth` datetime NOT NULL,
  `login` varchar(30) NOT NULL,
  `pass` varchar(30) NOT NULL,
  PRIMARY KEY (`id_store_manager`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `store_manager`
--

LOCK TABLES `store_manager` WRITE;
/*!40000 ALTER TABLE `store_manager` DISABLE KEYS */;
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
/*!40000 ALTER TABLE `warehouse_manager` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'hardware_store'
--

--
-- Dumping routines for database 'hardware_store'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-10-23  3:01:29
