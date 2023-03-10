USE [FoodyStore]
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [IsDeleted]) VALUES (1, N'coke', N'qwe', 0)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [IsDeleted]) VALUES (2, N'food', N'23', 0)
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Suppliers] ON 

INSERT [dbo].[Suppliers] ([SupplierId], [CompanyName], [Address], [Phone], [IsDeleted]) VALUES (1, N'abc', N'qwe', N'0908342111', 0)
INSERT [dbo].[Suppliers] ([SupplierId], [CompanyName], [Address], [Phone], [IsDeleted]) VALUES (2, N'tyu', N'asd', N'1234', 0)
INSERT [dbo].[Suppliers] ([SupplierId], [CompanyName], [Address], [Phone], [IsDeleted]) VALUES (3, N'Meat Lover', N'hcm', N'0908293434', 0)
SET IDENTITY_INSERT [dbo].[Suppliers] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ProductId], [ProductName], [SupplierId], [CategoryId], [QuantityPerUnit], [UnitPrice], [ProductImage], [IsDeleted]) VALUES (1, N'Salad', 1, 1, 755, 342.0000, N'images/Salad.jpg', 0)
INSERT [dbo].[Products] ([ProductId], [ProductName], [SupplierId], [CategoryId], [QuantityPerUnit], [UnitPrice], [ProductImage], [IsDeleted]) VALUES (2, N'vaef', 1, 1, 3, 45.0000, N'images/vaef.png', 1)
INSERT [dbo].[Products] ([ProductId], [ProductName], [SupplierId], [CategoryId], [QuantityPerUnit], [UnitPrice], [ProductImage], [IsDeleted]) VALUES (3, N'Steak', 1, 1, 122, 12.0000, N'images/aasd.jpg', 0)
INSERT [dbo].[Products] ([ProductId], [ProductName], [SupplierId], [CategoryId], [QuantityPerUnit], [UnitPrice], [ProductImage], [IsDeleted]) VALUES (4, N'vaef', 1, 1, 4, 345.0000, N'images/vaef.jpg', 1)
INSERT [dbo].[Products] ([ProductId], [ProductName], [SupplierId], [CategoryId], [QuantityPerUnit], [UnitPrice], [ProductImage], [IsDeleted]) VALUES (5, N'Omelette', 1, 1, 1243, 123.0000, N'images/Omelette.jpg', 0)
INSERT [dbo].[Products] ([ProductId], [ProductName], [SupplierId], [CategoryId], [QuantityPerUnit], [UnitPrice], [ProductImage], [IsDeleted]) VALUES (6, N'Cookie', 1, 1, 234, 234.0000, N'images/Cookie.jpg', 0)
INSERT [dbo].[Products] ([ProductId], [ProductName], [SupplierId], [CategoryId], [QuantityPerUnit], [UnitPrice], [ProductImage], [IsDeleted]) VALUES (7, N'Cake', 1, 1, 331, 235.0000, N'images/23.jpg', 0)
INSERT [dbo].[Products] ([ProductId], [ProductName], [SupplierId], [CategoryId], [QuantityPerUnit], [UnitPrice], [ProductImage], [IsDeleted]) VALUES (8, N'Chicken', 1, 1, 2305, 245.0000, N'images/Chicken.jpg', 0)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([Id], [Name], [IsDeleted]) VALUES (1, N'Administrator', 0)
INSERT [dbo].[Roles] ([Id], [Name], [IsDeleted]) VALUES (2, N'Member', 0)
INSERT [dbo].[Roles] ([Id], [Name], [IsDeleted]) VALUES (3, N'Seller', 0)
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Accounts] ON 

INSERT [dbo].[Accounts] ([AccountId], [Username], [RoleId], [Password], [Address], [Phone], [IsDeleted]) VALUES (1, N'Administrator', 1, N'123', N'', N'0908123456', 0)
INSERT [dbo].[Accounts] ([AccountId], [Username], [RoleId], [Password], [Address], [Phone], [IsDeleted]) VALUES (2, N'Member', 2, N'123', N'', N'0908123456', 0)
INSERT [dbo].[Accounts] ([AccountId], [Username], [RoleId], [Password], [Address], [Phone], [IsDeleted]) VALUES (3, N'Seller', 3, N'123', N'', N'0908123456', 0)
INSERT [dbo].[Accounts] ([AccountId], [Username], [RoleId], [Password], [Address], [Phone], [IsDeleted]) VALUES (4, N'tai001', 2, N'1', N'viet nam', N'0943221145', 0)
SET IDENTITY_INSERT [dbo].[Accounts] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([OrderId], [AccountId], [OrderDate], [RequiredDate], [ShippedDate], [Freight], [ShipAddress], [TotalPrice], [IsDeleted]) VALUES (3, 1, CAST(N'2022-03-02T00:00:00.0000000' AS DateTime2), CAST(N'2022-05-04T00:00:00.0000000' AS DateTime2), CAST(N'2022-06-04T00:00:00.0000000' AS DateTime2), N'erwrw', N'werwer', 452345.0000, 1)
INSERT [dbo].[Orders] ([OrderId], [AccountId], [OrderDate], [RequiredDate], [ShippedDate], [Freight], [ShipAddress], [TotalPrice], [IsDeleted]) VALUES (6, 2, CAST(N'2022-04-02T06:12:49.0000000' AS DateTime2), CAST(N'2022-04-03T10:03:40.0000000' AS DateTime2), CAST(N'2022-06-07T00:00:00.0000000' AS DateTime2), N'234', N'ewer', 234245.0000, 0)
INSERT [dbo].[Orders] ([OrderId], [AccountId], [OrderDate], [RequiredDate], [ShippedDate], [Freight], [ShipAddress], [TotalPrice], [IsDeleted]) VALUES (7, 2, CAST(N'2023-03-13T07:59:43.2714311' AS DateTime2), CAST(N'2023-03-23T07:59:43.2714322' AS DateTime2), CAST(N'2023-03-20T07:59:43.2714325' AS DateTime2), NULL, N'', 37970.0000, 0)
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
INSERT [dbo].[OrderDetails] ([OrderId], [ProductId], [UnitPrice], [Quantity]) VALUES (3, 5, 343.0000, 5)
INSERT [dbo].[OrderDetails] ([OrderId], [ProductId], [UnitPrice], [Quantity]) VALUES (7, 7, 235.0000, 123)
INSERT [dbo].[OrderDetails] ([OrderId], [ProductId], [UnitPrice], [Quantity]) VALUES (7, 8, 245.0000, 37)
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20230224062918_initdb', N'6.0.14')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20230224063020_initdb_2', N'6.0.14')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20230225020344_add_delete_status', N'6.0.14')
GO
