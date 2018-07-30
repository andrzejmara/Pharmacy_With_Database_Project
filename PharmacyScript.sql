CREATE DATABASE Pharmacy

CREATE TABLE Medicines(
ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
Name VARCHAR(256),
Manufacturer VARCHAR(256), 
Price DECIMAL, 
Amount INT,
WithPrescription BIT
);

CREATE TABLE Prescriptions(
ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
CustomerName varCHAR(256), 
PESEL  varchar(256),
PrescriptionNumber INT
);

ALTER TABLE Prescriptions 
ADD CONSTRAINT PESEL_LENGTH CHECK (DATALENGTH(PESEL) = 10)

CREATE TABLE ORDERS (
ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
PrescriptionID INT,
MedicineID INT NOT NULL,
Date DATETIME,
Amount INT
);

ALTER TABLE ORDERS 
ADD CONSTRAINT FK_Orders_Prescriptions FOREIGN KEY(PrescriptionID)
REFERENCES Prescriptions (ID);
ALTER TABLE ORDERS 
ADD CONSTRAINT FK_Orders_Medicines FOREIGN KEY(MedicineID)
REFERENCES Medicines (ID);

INSERT INTO Medicines(Name, Manufacturer, Price, Amount, WithPrescription) Values ('Ketonal', 'BMH', 23.99, 8, 0)
INSERT INTO Medicines(Name, Manufacturer, Price, Amount, WithPrescription) Values ('Poxidal', 'Frayer', 66.30, 13, 1);
INSERT INTO Medicines(Name, Manufacturer, Price, Amount, WithPrescription) Values ('Mezoskin', 'Bauer Cosmetics', 23.99, 5, 0);
INSERT INTO Medicines(Name, Manufacturer, Price, Amount, WithPrescription) Values ('Hitorin', 'BMH', 99.75, 23, 1);
INSERT INTO Medicines(Name, Manufacturer, Price, Amount, WithPrescription) Values ('Lekomanix', 'Frayer', 78.99, 15, 0)
INSERT INTO Prescriptions(CustomerName, PESEL, PrescriptionNumber) Values ('Ryszard Smith', 6284729007, 36)
INSERT INTO Orders(PrescriptionID, MedicineID, Amount) Values (4, 9, 10);
delete from Prescriptions where CustomerName like ('Rysz%');



