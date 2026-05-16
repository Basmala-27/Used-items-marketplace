EXEC sp_MSforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all";
GO
PRINT 'Importing AspNetRoles...';
BEGIN TRY
    SET IDENTITY_INSERT [AspNetRoles] ON;
END TRY
BEGIN CATCH
END CATCH
INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'951c6510-b90f-4b82-907e-d6864cb8c169', N'Admin', N'ADMIN', NULL);
GO
BEGIN TRY
    SET IDENTITY_INSERT [AspNetRoles] OFF;
END TRY
BEGIN CATCH
END CATCH
GO
PRINT 'Importing AspNetUsers...';
BEGIN TRY
    SET IDENTITY_INSERT [AspNetUsers] ON;
END TRY
BEGIN CATCH
END CATCH
INSERT INTO [AspNetUsers] ([Id], [Name], [Location], [ProfileImage], [TrustScore], [Rating], [CreatedAt], [WalletBalance], [PendingBalance], [IsBlocked], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'088efc4f-c677-4ef1-b4f1-746cbea56d89', N'Tasneem Abdulrahman', N'', N'/images/profiles/6901a4e7-e70c-4729-a2e0-cf3727e54e99.jpg', 0.0, 0, N'2026-05-09 21:24:41.4782826', 0, 0, 0, N'wwww95287@gmail.com', N'WWWW95287@GMAIL.COM', N'wwww95287@gmail.com', N'WWWW95287@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEOZ0AkLjtqubbGvksfX6R48NTSnsB15MjCKUyjxy2vvKCFqJ2zFfQh5WhjNIOa2IGg==', N'STW24STONIZKJM2UN47N7JFZ2TVDCNWC', N'aecde2ac-767a-44c9-943b-7c93cdcd4941', N'01001072692', 0, 0, NULL, 1, 0);
INSERT INTO [AspNetUsers] ([Id], [Name], [Location], [ProfileImage], [TrustScore], [Rating], [CreatedAt], [WalletBalance], [PendingBalance], [IsBlocked], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'0c4abf45-6a01-41c2-82b6-c3642e7b3cf6', N'Basmala Mohammed', N'', N'/uploads/default-avatar.png', 0.2, 1, N'2026-05-09 21:27:14.5997632', 500, 0, 0, N'basmala7@gmail.com', N'BASMALA7@GMAIL.COM', N'basmala7@gmail.com', N'BASMALA7@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEPxX28nmvDekzJZZuMY0HKDU75MUWVpNvg2jsVpVQX5xEdbEF+sLkK9djywNoHD5gg==', N'677U5ZSYDFBVFEUW6SJWE3CHYWAC5YJ7', N'4c80c095-bbae-4786-b718-cb29ad7b0acd', N'01067618897', 0, 0, NULL, 1, 0);
INSERT INTO [AspNetUsers] ([Id], [Name], [Location], [ProfileImage], [TrustScore], [Rating], [CreatedAt], [WalletBalance], [PendingBalance], [IsBlocked], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'ea6341a9-9414-4730-958f-e553691a55ee', N'Basmala', N'', N'/images/profiles/a5472827-c4e2-46f2-b443-e2fc271004af.jpg', 0.0, 0, N'2026-05-09 21:32:37.1217801', 1250, 0, 0, N'basmala01067618897@gmail.com', N'BASMALA01067618897@GMAIL.COM', N'basmala01067618897@gmail.com', N'BASMALA01067618897@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAECnHZ5Jf0jM6k2VvrV8G20cIUCpFk0ErU+wVS5UdyKSwP3GcUSKpG0+3+9WkSR3FcA==', N'AA6W2GP7VV5TBMCPXL354DBNBNUGWSRG', N'df876f24-2edb-4a3d-8769-b1e5962d552d', N'01067618897', 0, 0, NULL, 1, 0);
INSERT INTO [AspNetUsers] ([Id], [Name], [Location], [ProfileImage], [TrustScore], [Rating], [CreatedAt], [WalletBalance], [PendingBalance], [IsBlocked], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'0a76d56b-2546-4556-965d-633100430c75', N'Tasnemaa', N'', N'/uploads/a12d893c-9e26-497e-880c-24a333b4c3f5.jpg', 0.2, 1, N'2026-05-09 21:34:18.8200042', 1400, 0, 0, N'tasneem@gmail.com', N'TASNEEM@GMAIL.COM', N'tasneem@gmail.com', N'TASNEEM@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEFgfubUWC8WYGoNy1Dp8TpzHVd2dr5BYzw7yUiwMyn5U2aofC5BAja0+o5Q3qMz6eA==', N'DODC45LDCL35TYLNBQTIS2R2RBQMERFB', N'983790d3-f9d3-41e9-88d9-6e4c0dd0ad49', N'01001072692', 0, 0, NULL, 1, 0);
INSERT INTO [AspNetUsers] ([Id], [Name], [Location], [ProfileImage], [TrustScore], [Rating], [CreatedAt], [WalletBalance], [PendingBalance], [IsBlocked], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'8bdb2289-8573-49d2-ba95-e7b379c42db8', N'Tasneem', N'', N'/uploads/default-avatar.png', 0.0, 0, N'2026-05-09 22:36:03.5259789', 0, 0, 0, N'tasneem@admin.com', N'TASNEEM@ADMIN.COM', N'tasneem@admin.com', N'TASNEEM@ADMIN.COM', 0, N'AQAAAAIAAYagAAAAECTXHf0Q64lFySVqHJOewtYQFfy6W3zFEQG57bRYnS6q6r9zGKh1T584qNhYFOre+w==', N'N4G6S3ZZXHZIGXGBRTGDUTQZQODKQHP4', N'd87bc106-9167-404c-831a-d6bdad37504b', N'01001072692', 0, 0, NULL, 1, 0);
INSERT INTO [AspNetUsers] ([Id], [Name], [Location], [ProfileImage], [TrustScore], [Rating], [CreatedAt], [WalletBalance], [PendingBalance], [IsBlocked], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'Arwa Khalid', N'', N'/uploads/4ab11384-49fe-407c-8c7d-e6370339435a.jpg', 0.2, 2, N'2026-05-12 22:16:42.1721321', 2689.99, 0, 0, N'arwa.rorokh25@gmail.com', N'ARWA.ROROKH25@GMAIL.COM', N'arwa.rorokh25@gmail.com', N'ARWA.ROROKH25@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEA9TXlkyjhJljbdD6mw1hP3UdLqnzCud5+5TAuKNh0V5mkL89cTfyTq482FZ/dGsZA==', N'FB7UJTSY3UWQNAXL2WGTNT5ZKL6GQCY6', N'54907bac-2b95-4217-8cab-ab252267dd29', N'01159419122', 0, 0, NULL, 1, 0);
INSERT INTO [AspNetUsers] ([Id], [Name], [Location], [ProfileImage], [TrustScore], [Rating], [CreatedAt], [WalletBalance], [PendingBalance], [IsBlocked], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'638d0c22-6df4-4edf-8625-4942a31dce12', N'Al Hassan A.Thabet', N'', N'/uploads/default-avatar.png', 0.0, 0, N'2026-05-13 02:16:57.7666861', 0, 0, 0, N'shant1blodm1@gmail.com', N'SHANT1BLODM1@GMAIL.COM', N'shant1blodm1@gmail.com', N'SHANT1BLODM1@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEFED8vR1mi5ZoMGseP+89jqHpDslNvpfQNhwmWcAJOfdHH7Clm7ak3OY+gO8UFHdhA==', N'SFJWGID7YEUC445RPVWE5GQO5IUBCAMP', N'469c00b1-472a-4ac1-b2ed-686abafa3df4', N'01114432747', 0, 0, NULL, 1, 0);
INSERT INTO [AspNetUsers] ([Id], [Name], [Location], [ProfileImage], [TrustScore], [Rating], [CreatedAt], [WalletBalance], [PendingBalance], [IsBlocked], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'c474177c-799a-44b0-b74d-33f716a3a5fc', N'Ereny Shehata', N'', N'/uploads/default-avatar.png', 0.0, 0, N'2026-05-13 16:16:42.1181532', 0, 0, 0, N'ereny@gmail.com', N'ERENY@GMAIL.COM', N'ereny@gmail.com', N'ERENY@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEMWkYobj3ajglMq6GkVAdb9R9QvtbvekSfsK+u0EVigMjtW6qVX25X5epn0e/bwS9A==', N'2RBPXRISKAQRXNKZHV4ERB5JW2FHQPG7', N'5de37aac-5ca4-4718-ba9d-5a12132e370c', N'0123456', 0, 0, NULL, 1, 0);
INSERT INTO [AspNetUsers] ([Id], [Name], [Location], [ProfileImage], [TrustScore], [Rating], [CreatedAt], [WalletBalance], [PendingBalance], [IsBlocked], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', N'Arwa Khalid', N'', N'/uploads/default-avatar.png', 0.6000000000000001, 4, N'2026-05-13 20:20:14.5517381', 17809.98, 0, 0, N'arwakhalid.abdelrady@gmail.com', N'ARWAKHALID.ABDELRADY@GMAIL.COM', N'arwakhalid.abdelrady@gmail.com', N'ARWAKHALID.ABDELRADY@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEHPmsGr5ZQ1oIiyVI5/RxvlqfL6WE84hDmyOhg8OFEA4GDL+3r1wYROCn5FPs3obMQ==', N'W3KGNJP5EJNB2VQP5JT2TZF7ZV4L6DH6', N'd06f8630-cdd0-4143-93ce-5c485f464b3d', N'01159419122', 0, 0, NULL, 1, 0);
INSERT INTO [AspNetUsers] ([Id], [Name], [Location], [ProfileImage], [TrustScore], [Rating], [CreatedAt], [WalletBalance], [PendingBalance], [IsBlocked], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'da368678-4a4a-4b5c-bd6e-993c0834dbb7', N'Gory', N'', N'/uploads/default-avatar.png', 0.0, 0, N'2026-05-13 21:23:35.5913495', 200, 400, 0, N'ggg@gmail.com', N'GGG@GMAIL.COM', N'ggg@gmail.com', N'GGG@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAENk5jrDzE2yiSAH15XrSiKD/wYjBDmCBwzVEO9FDsipQLvQ5cLRerAqWrgwmX+XxNg==', N'DY2NA5CDZKWLLW2PP47KS6RH3HKU6IBL', N'ab853034-9d8a-41a1-9c17-a3319f0e8e62', N'01001072692', 0, 0, NULL, 1, 0);
INSERT INTO [AspNetUsers] ([Id], [Name], [Location], [ProfileImage], [TrustScore], [Rating], [CreatedAt], [WalletBalance], [PendingBalance], [IsBlocked], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'c5827b40-66e6-453c-8718-fe3e52619680', N'Basmala Mohamed', N'', N'/uploads/default-avatar.png', 0.0, 0, N'2026-05-13 22:24:07.934689', 0, 0, 0, N'basmala11@gmail.com', N'BASMALA11@GMAIL.COM', N'basmala11@gmail.com', N'BASMALA11@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEJtosFGRZw5QO/3sHBKyZVCk4QZ1N8eZPPON57SACffS7ytSXpB6j20FN66iWIApMQ==', N'K3JI53PE5PX5JT3CBOP5R7NYWPZDO6EE', N'abba8387-ad82-425c-92b9-07f6de9b6df4', N'01067618897', 0, 0, NULL, 1, 0);
INSERT INTO [AspNetUsers] ([Id], [Name], [Location], [ProfileImage], [TrustScore], [Rating], [CreatedAt], [WalletBalance], [PendingBalance], [IsBlocked], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'd1bbac46-7d40-4800-8c4a-a560cb499c5f', N'Tasneem', N'', N'/uploads/default-avatar.png', 0.0, 0, N'2026-05-13 23:50:52.173678', 9000, 0, 0, N'Tasneem7122005@gmail.com', N'TASNEEM7122005@GMAIL.COM', N'Tasneem7122005@gmail.com', N'TASNEEM7122005@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAENKoEEOov4xoeMAptoP4XG43yFfuUCakGkNKSmzm87dq7qBPoaFS6Tb97iOysyXs7Q==', N'KWPKDVTLS3VQQVJNDJHKRONJMVAC7UZL', N'9ed2c8bf-d100-4190-afee-35771f57164b', N'01001072692', 0, 0, NULL, 1, 0);
GO
BEGIN TRY
    SET IDENTITY_INSERT [AspNetUsers] OFF;
END TRY
BEGIN CATCH
END CATCH
GO
PRINT 'Importing Categories...';
BEGIN TRY
    SET IDENTITY_INSERT [Categories] ON;
END TRY
BEGIN CATCH
END CATCH
INSERT INTO [Categories] ([CategoryID], [Name], [ImageUrl]) VALUES (1, N'Electronics', N'/images/categories/electronics.jpg');
INSERT INTO [Categories] ([CategoryID], [Name], [ImageUrl]) VALUES (2, N'Furniture', N'/images/categories/furniture.jpg');
INSERT INTO [Categories] ([CategoryID], [Name], [ImageUrl]) VALUES (3, N'Clothes', N'/images/categories/fashion.jpg');
INSERT INTO [Categories] ([CategoryID], [Name], [ImageUrl]) VALUES (4, N'Phones', N'/images/categories/phones.jpg');
INSERT INTO [Categories] ([CategoryID], [Name], [ImageUrl]) VALUES (5, N'Laptops', N'/images/categories/laptops.jpg');
INSERT INTO [Categories] ([CategoryID], [Name], [ImageUrl]) VALUES (6, N'Accessories', N'/images/categories/accessories.jpg');
INSERT INTO [Categories] ([CategoryID], [Name], [ImageUrl]) VALUES (7, N'Gaming', N'/images/categories/gaming.jpg');
INSERT INTO [Categories] ([CategoryID], [Name], [ImageUrl]) VALUES (8, N'Vehicles', N'/images/categories/vehicles.jpg');
INSERT INTO [Categories] ([CategoryID], [Name], [ImageUrl]) VALUES (9, N'Study Materials', N'/images/categories/study.jpg');
INSERT INTO [Categories] ([CategoryID], [Name], [ImageUrl]) VALUES (10, N'Others', N'/images/categories/others.jpg');
GO
BEGIN TRY
    SET IDENTITY_INSERT [Categories] OFF;
END TRY
BEGIN CATCH
END CATCH
GO
PRINT 'Importing AspNetUserRoles...';
BEGIN TRY
    SET IDENTITY_INSERT [AspNetUserRoles] ON;
END TRY
BEGIN CATCH
END CATCH
INSERT INTO [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'8bdb2289-8573-49d2-ba95-e7b379c42db8', N'951c6510-b90f-4b82-907e-d6864cb8c169');
INSERT INTO [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'ea6341a9-9414-4730-958f-e553691a55ee', N'951c6510-b90f-4b82-907e-d6864cb8c169');
INSERT INTO [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'088efc4f-c677-4ef1-b4f1-746cbea56d89', N'951c6510-b90f-4b82-907e-d6864cb8c169');
INSERT INTO [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', N'951c6510-b90f-4b82-907e-d6864cb8c169');
GO
BEGIN TRY
    SET IDENTITY_INSERT [AspNetUserRoles] OFF;
END TRY
BEGIN CATCH
END CATCH
GO
PRINT 'Importing Notifications...';
BEGIN TRY
    SET IDENTITY_INSERT [Notifications] ON;
END TRY
BEGIN CATCH
END CATCH
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (1, N'0a76d56b-2546-4556-965d-633100430c75', 2, N'🛒 New buy request for your item "Tapo c200 360-degree smart wi-fi pan and tilt camera, 1080 p - white" at $800.00 — Accept or reject the request', N'/Transactions/SellerRequests', 1, 1, N'2026-05-09 23:15:21.8085149', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (2, N'0a76d56b-2546-4556-965d-633100430c75', 2, N'🛒 New buy request for your item "Tapo c200 360-degree smart wi-fi pan and tilt camera, 1080 p - white" at $800.00 — Accept or reject the request', N'/Transactions/SellerRequests', 2, 1, N'2026-05-12 20:20:23.5817208', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (3, N'0a76d56b-2546-4556-965d-633100430c75', 2, N'🛒 New buy request for your item "Tapo c200 360-degree smart wi-fi pan and tilt camera, 1080 p - white" at $800.00 — Accept or reject the request', N'/Transactions/SellerRequests', 3, 1, N'2026-05-12 20:20:50.892729', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (4, N'0a76d56b-2546-4556-965d-633100430c75', 2, N'🛒 New buy request for your item "Tapo c200 360-degree smart wi-fi pan and tilt camera, 1080 p - white" at $800.00 — Accept or reject the request', N'/Transactions/SellerRequests', 4, 1, N'2026-05-12 20:25:09.1433975', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (5, N'0c4abf45-6a01-41c2-82b6-c3642e7b3cf6', 2, N'🛒 New buy request for your item "AA" at $500.00 — Accept or reject the request', N'/Transactions/SellerRequests', 5, 1, N'2026-05-12 20:46:50.9710995', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (6, N'ea6341a9-9414-4730-958f-e553691a55ee', 2, N'❌ The seller rejected your buy request for "AA". $500.00 has been refunded to your wallet.', N'/Transactions/MyOrders', 5, 1, N'2026-05-12 20:48:38.1158602', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (7, N'0c4abf45-6a01-41c2-82b6-c3642e7b3cf6', 2, N'🛒 New buy request for your item "AA" at $500.00 — Accept or reject the request', N'/Transactions/SellerRequests', 6, 0, N'2026-05-12 20:58:28.3969377', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (8, N'0a76d56b-2546-4556-965d-633100430c75', 2, N'🛒 New buy request for your item "Tapo c200 360-degree smart wi-fi pan and tilt camera, 1080 p - white" at $800.00 — Accept or reject the request', N'/Transactions/SellerRequests', 7, 1, N'2026-05-12 20:58:41.0619419', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (9, N'0c4abf45-6a01-41c2-82b6-c3642e7b3cf6', 2, N'🛒 New buy request for your item "AA" at $500.00 — Accept or reject the request', N'/Transactions/SellerRequests', 8, 0, N'2026-05-12 21:23:59.9971164', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (10, N'ea6341a9-9414-4730-958f-e553691a55ee', 2, N'❌ The seller rejected your buy request for "AA". $500.00 has been refunded to your wallet.', N'/Transactions/MyOrders', 8, 1, N'2026-05-12 21:24:31.5148615', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (11, N'0c4abf45-6a01-41c2-82b6-c3642e7b3cf6', 2, N'🛒 New buy request for your item "AA" at $500.00 — Accept or reject the request', N'/Transactions/SellerRequests', 9, 0, N'2026-05-12 21:34:16.8696221', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (12, N'ea6341a9-9414-4730-958f-e553691a55ee', 2, N'❌ The seller rejected your buy request for "AA". $500.00 has been refunded to your wallet.', N'/Transactions/MyOrders', 9, 1, N'2026-05-12 21:34:54.0969356', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (13, N'0c4abf45-6a01-41c2-82b6-c3642e7b3cf6', 2, N'🛒 New buy request for your item "AA" at $500.00 — Accept or reject the request', N'/Transactions/SellerRequests', 10, 0, N'2026-05-12 21:45:48.8777829', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (14, N'ea6341a9-9414-4730-958f-e553691a55ee', 2, N'✅ The seller accepted your buy request for "AA"! Shipping in progress. Confirm receipt to release payment.', N'/Transactions/MyOrders', 10, 1, N'2026-05-12 21:46:14.3045803', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (15, N'0c4abf45-6a01-41c2-82b6-c3642e7b3cf6', 6, N'✅ The buyer confirmed receiving "AA". The final $250.00 (50%) has been transferred to your wallet!', N'/Profile#transactions', 10, 0, N'2026-05-12 21:47:35.3166827', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (16, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 3, N'💬 New message from Arwa Khalid.', N'/Chat/Chat/1', 1, 1, N'2026-05-13 20:35:24.4797555', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (17, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 2, N'🛒 New buy request for your item "Straight Leg Jeans" at $500.00 — Accept or reject the request', N'/Transactions/SellerRequests', 11, 1, N'2026-05-13 20:36:00.329392', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (18, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 3, N'💬 New message from Arwa Khalid.', N'/Chat/Chat/1', 1, 1, N'2026-05-13 20:37:01.5422327', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (19, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 2, N'❌ The seller rejected your buy request for "Straight Leg Jeans". $500.00 has been refunded to your wallet.', N'/Transactions/MyOrders', 11, 1, N'2026-05-13 20:37:12.3574285', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (20, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 2, N'🛒 New buy request for your item "Straight Leg Jeans" at $500.00 — Accept or reject the request', N'/Transactions/SellerRequests', 12, 1, N'2026-05-13 20:38:05.9964085', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (21, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 2, N'✅ The seller accepted your buy request for "Straight Leg Jeans"! Shipping in progress. Confirm receipt to release payment.', N'/Transactions/MyOrders', 12, 1, N'2026-05-13 20:38:55.9182779', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (22, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 6, N'✅ The buyer confirmed receiving "Straight Leg Jeans". The final $250.00 (50%) has been transferred to your wallet!', N'/Profile#transactions', 12, 1, N'2026-05-13 20:39:31.0077196', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (23, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 2, N'🛒 New buy request for your item "SONY" at $6666.00 — Accept or reject the request', N'/Transactions/SellerRequests', 13, 1, N'2026-05-13 20:42:59.9227743', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (24, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 2, N'✅ The seller accepted your buy request for "SONY"! Shipping in progress. Confirm receipt to release payment.', N'/Transactions/MyOrders', 13, 1, N'2026-05-13 20:43:24.7573689', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (25, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 6, N'✅ The buyer confirmed receiving "SONY". The final $3333.00 (50%) has been transferred to your wallet!', N'/Profile#transactions', 13, 1, N'2026-05-13 20:43:47.1293399', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (26, N'0a76d56b-2546-4556-965d-633100430c75', 2, N'🛒 New buy request for your item "Tapo c200 360-degree smart wi-fi pan and tilt camera, 1080 p - white" at $800.00 — Accept or reject the request', N'/Transactions/SellerRequests', 14, 1, N'2026-05-13 21:25:06.5983154', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (27, N'da368678-4a4a-4b5c-bd6e-993c0834dbb7', 2, N'✅ The seller accepted your buy request for "Tapo c200 360-degree smart wi-fi pan and tilt camera, 1080 p - white"! Shipping in progress. Confirm receipt to release payment.', N'/Transactions/MyOrders', 14, 0, N'2026-05-13 21:25:37.263285', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (28, N'0a76d56b-2546-4556-965d-633100430c75', 3, N'💬 New message from Tasneem.', N'/Chat/Chat/2', 2, 1, N'2026-05-13 23:53:23.1975056', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (29, N'0a76d56b-2546-4556-965d-633100430c75', 2, N'🛒 New buy request for your item "Brand : Logitech Connectivity : USB Device Type : Over-Ear Headphones Part Number : 981-000406 Color : Black" at $1000.00 — Accept or reject the request', N'/Transactions/SellerRequests', 15, 1, N'2026-05-13 23:54:06.2478565', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (30, N'd1bbac46-7d40-4800-8c4a-a560cb499c5f', 2, N'✅ The seller accepted your buy request for "Brand : Logitech Connectivity : USB Device Type : Over-Ear Headphones Part Number : 981-000406 Color : Black"! Shipping in progress. Confirm receipt to release payment.', N'/Transactions/MyOrders', 15, 1, N'2026-05-13 23:54:57.3255144', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (31, N'0a76d56b-2546-4556-965d-633100430c75', 6, N'✅ The buyer confirmed receiving "Brand : Logitech Connectivity : USB Device Type : Over-Ear Headphones Part Number : 981-000406 Color : Black". The final $500.00 (50%) has been transferred to your wallet!', N'/Profile#transactions', 15, 1, N'2026-05-13 23:56:14.6043084', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (32, N'd1bbac46-7d40-4800-8c4a-a560cb499c5f', 3, N'💬 New message from Tasnemaa.', N'/Chat/Chat/2', 2, 0, N'2026-05-13 23:58:25.9118747', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (33, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 5, N'🔄 You have a new swap request for your item "Samsung".', N'/Transactions/SellerRequests', 1, 1, N'2026-05-14 02:12:32.3104545', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (34, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 5, N'✅ Your swap request for "Samsung" has been approved.', N'/Profile#transactions', 1, 1, N'2026-05-14 02:13:16.1319906', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (35, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 2, N'🔄 Swap Complete! "Samsung" is now yours.', N'/SwapRequests/MyRequests', 1, 1, N'2026-05-14 02:13:16.1395068', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (36, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 2, N'🛒 New buy request for your item "SAM" at $666.00 — Accept or reject the request', N'/Transactions/SellerRequests', 16, 1, N'2026-05-14 02:18:17.8683198', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (37, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 2, N'✅ The seller accepted your buy request for "SAM"! Shipping in progress. Contact: 01159419122. Confirm receipt to release payment.', N'/Transactions/MyOrders', 16, 1, N'2026-05-14 02:18:46.4118731', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (38, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 6, N'✅ The buyer confirmed receiving "SAM". The final $333.00 (50%) has been transferred to your wallet!', N'/Profile#transactions', 16, 1, N'2026-05-14 02:19:19.0533632', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (39, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 3, N'💬 New message from Arwa Khalid.', N'/Chat/Chat/3', 3, 1, N'2026-05-14 15:03:36.8774695', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (40, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 2, N'🛒 New buy request for your item "Lmkm," at $22.00 — Accept or reject the request', N'/Transactions/SellerRequests', 17, 1, N'2026-05-14 15:03:48.1169617', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (41, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 2, N'✅ The seller accepted your buy request for "Lmkm,"! Shipping in progress. Contact: 01159419122. Confirm receipt to release payment.', N'/Transactions/MyOrders', 17, 1, N'2026-05-14 15:05:20.8689989', NULL);
INSERT INTO [Notifications] ([NotificationID], [UserID], [Type], [MessageText], [TargetUrl], [RelatedEntityID], [IsRead], [CreatedAt], [ApplicationUserId]) VALUES (42, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 6, N'✅ The buyer confirmed receiving "Lmkm,". The final $11.00 (50%) has been transferred to your wallet!', N'/Profile#transactions', 17, 1, N'2026-05-14 15:06:22.9667615', NULL);
GO
BEGIN TRY
    SET IDENTITY_INSERT [Notifications] OFF;
END TRY
BEGIN CATCH
END CATCH
GO
PRINT 'Importing Items...';
BEGIN TRY
    SET IDENTITY_INSERT [Items] ON;
END TRY
BEGIN CATCH
END CATCH
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (1, N'SanDisk Ultra Flair 128GB, USB 3.0 Flash Drive, 150MB/s', 1, 1, N'128GB storage capacity
High-speed USB 3.0 performance of up to 150 MB/s
Robust and at the same time elegant metal design
With SanDisk secure access software for password protection and file encryption
Interfaces: USB 3.2 1st Gen (USB 3.0)', 400, N'Like New', N'Assuit', 0, N'Sale', N'0a76d56b-2546-4556-965d-633100430c75', 1, N'2026-05-09 21:38:02.3003915', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (2, N'Joyroom JR-QP191 22.5W Power Bank 10000mAh With Large Digital Display Supports PD3.0,QC3.0 - Black|12 Months Warranty', 0, 1, N'22.5W Fast Charging Output – Delivers efficient high-speed charging for compatible smartphones and devices.
10000mAh Reliable Capacity – Provides dependable backup power for daily use, travel, or emergencies.
LCD Digital Display – Clearly shows the remaining battery percentage for accurate power monitoring.
Compact & Portable Design – Lightweight size makes it easy to carry in your pocket or bag.
Safe & Durable Performance – Built-in protection features help guard against overheating, overcharging, and short circuits.', 500, N'Very Good', N'Assuit', 0, N'Sale', N'0a76d56b-2546-4556-965d-633100430c75', 1, N'2026-05-09 21:40:30.2793986', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (3, N'Brand : Logitech Connectivity : USB Device Type : Over-Ear Headphones Part Number : 981-000406 Color : Black', 1, 1, N'Brand : Logitech
Connectivity : USB
Device Type : Over-Ear Headphones
Part Number : 981-000406
Color : Black', 1000, N'Good', N'Assuit', 4, N'Sale', N'0a76d56b-2546-4556-965d-633100430c75', 1, N'2026-05-09 21:42:22.6032165', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (4, N'Tapo c200 360-degree smart wi-fi pan and tilt camera, 1080 p - white', 1, 1, N'Smart Wi-Fi Pan and Tilt Camera, 1080 P
Country of origin ‏ : ‎ China
suitable color
Compatible with multiple devices
Unique and fashionable design.', 800, N'Very Good', N'Assuit', 3, N'Sale', N'0a76d56b-2546-4556-965d-633100430c75', 1, N'2026-05-09 21:44:33.8830136', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (5, N'AA', 1, 1, N'ايوه ', 500, N'Like New', N'Assuit', 4, N'Sale', N'ea6341a9-9414-4730-958f-e553691a55ee', 10, N'2026-05-12 20:26:47.5402748', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (8, N'Mercedes-Benz G-Class', 1, 1, N'The 2026 Mercedes-Benz G-Class (or G-Wagen) is an iconic luxury SUV, blending extreme off-road capability with high-end luxury, available in G 550 and AMG G 63 variants.', 3000000, N'Like New', N'Cairo', 0, N'Sale', N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 8, N'2026-05-12 22:21:48.3288129', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (9, N'Nissan GT-R (R35)', 0, 0, N'The 2024/2025 Nissan GT-R (R35) is a renowned high-performance "supercar killer" featuring a 3.8L twin-turbo V6 engine, producing 565 hp (600 hp in the NISMO trim). Known for its advanced all-wheel-drive system and blistering 0-60 mph times under 3 seconds, it blends daily drivability with track-focused speed. ', 1990000, N'Like New', N'Cairo', 0, N'Sale', N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 8, N'2026-05-12 22:23:03.0036558', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (10, N'Kitchen Ladder Chair', 1, 0, N'High-quality folding kitchen ladder chair

', 1400, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 2, N'2026-05-13 16:21:41.2753373', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (11, N'Modern C-Shaped 2-Tier Side Table', 1, 1, N'Enhance your living space with this stylish and functional C-shaped side table. Featuring a dual-layer design with a rich dark wood finish and a sturdy black metal frame, this table is perfect for modern homes. Its unique structure allows it to slide conveniently under sofas or beds, bringing your snacks, drinks, or laptop closer to you while saving valuable floor space.', 2000, N'Very Good', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 2, N'2026-05-13 16:25:21.8386031', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (12, N'Two-piece wooden and white kitchen unit', 1, 1, N'Type: Kitchen Wall Cabinets
Color: White Wood
Material: Plywood
Number of Pieces: 2
Upper Unit Size in cm:
Width: 120
Height: 60
Depth: 30
Lower Unit Size in cm:
Width: 120
Height: 90
Depth: 40', 4000, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 2, N'2026-05-13 16:26:44.8853531', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (13, N'Coffee corner, white and wood color', 1, 1, N'Coffee corner, mdf, white and wood color, Dimensions: Width 60cm, Height 82cm, Depth 30cm', 4000, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 2, N'2026-05-13 16:29:04.2402223', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (14, N'Clue Dropped-Shoulder Relaxed Shirt', 0, 0, N'This viscose shirt is designed for effortless comfort with a relaxed, easy silhouette. Featuring a classic collar, button-down front, and dropped shoulders, it offers a soft drape and a clean, modern look. The solid pattern and round hem make it a versatile piece that works perfectly for both casual and polished styling.

Styling Options:

Tuck it into tailored trousers with loafers for a refined, everyday outfit.
Wear it loose over jeans with sneakers for a relaxed, casual look.
Layer it under a lightweight knit or vest for an easy, stylish layered ensemble.', 400, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 3, N'2026-05-13 16:31:51.7918308', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (15, N'Defacto Harry Potter Polo Collar Printed Oversize T-Shirt', 0, 0, N'As DeFacto, we set out to bring a fresh perspective to Turkish fashion and to bring our high quality and unique designs to consumers all over the world in 2005. Since the day the company was founded, we have accomplished important works and achievements by keeping our excitement alive.', 800, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 3, N'2026-05-13 16:33:04.7159762', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (17, N'Striped Imported Poplin Shirt with Wide Sleeves - blue Sky and white', 0, 0, N'Crafted from premium imported poplin fabric, it offers a soft feel against the skin and exceptional durability. The vertical stripe pattern provides a sophisticated aesthetic while creating a flattering, elongated silhouette. Whether you''re heading to the office or a casual outing, this shirt is your go-to choice for comfort and elegance.', 320, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 3, N'2026-05-13 16:43:00.5294582', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (18, N'Women''s Gabardine Mini Trench Vest with Waist Belt - Open Sides - black', 0, 0, N'​This unique piece is designed for the woman who loves a mix of classic elegance and contemporary street style. Featuring an innovative open-side design, this vest offers a fluid silhouette and ultimate freedom of movement, making it the perfect layering essential.', 380, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 3, N'2026-05-13 16:44:34.3429788', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (19, N'Sony Dualshock 4 PS4 Controller - Black', 0, 0, N'PlayStation 4 Wireless Gamepad has refined analog sticks and triggers, Light bar, USB battery charging, and more other features in the DualShock 4 for PlayStation 4 with black colour.', 4299, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 7, N'2026-05-13 16:56:42.4935789', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (20, N'Atari Atari- Mario multi-game console with 453 games in the shape of a PlayStation 5', 0, 0, N'Atari Mario game contains 453 games in a new form in the form of PlayStation Five.', 500, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 7, N'2026-05-13 17:03:24.2692675', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (21, N'Atari tape Multiple Atari games dedicated to the Atari Super Mario console, new action and adventure games', 0, 0, N'The ATARI tape Super Mario Super 8-Bit Game Console is a classic gaming console that provides a unique experience for retro game fans. This unit comes loaded with a variety of games, making it the perfect choice for family entertainment or with friends.', 70, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 7, N'2026-05-13 17:04:26.5890376', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (22, N'Sup Game Boy Plus Atari Mario Multigame Programmer 400 in 1 Game', 0, 0, N'Sup Game Boy Plus Atari Mario Multigame Programmer is the ultimate handheld gaming console designed for retro gaming enthusiasts. With a vast library of 400 games, it promises endless entertainment for players of all ages.', 400, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 7, N'2026-05-13 17:06:00.3096603', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (23, N'Samsung 55-Inch Neo QLED 4K Smart TV QA55QN85FAUXEG (2025 Model) – Dolby Atmos, Anti-Reflection', 0, 0, N'Dolby Atmos for cinematic sound
Quantum HDR 24x
Wide Viewing Angle
Anti-Reflection technology
Smart calibration features


Enjoy a premium viewing experience with the Samsung 55-Inch Neo QLED 4K Smart TV. Powered by Neo Quantum HDR, it offers rich contrast and vivid colors, with Dolby Atmos sound enhancing your audio experience. Equipped with Motion Xcelerator and Object Tracking Sound, this model provides clarity, depth, and immersive sound. The ultra-thin profile and sleek design make this TV a perfect fit for any modern home setup.', 43000, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 1, N'2026-05-13 17:08:40.2155837', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (24, N'Plastic Sealing Machine White Manual Pressure Bag Sealing Machine White', 0, 0, N'The Small Plastic Sealing Machine is designed to help you keep your food fresh and safe with its efficient sealing capabilities. This manual pressure bag sealing machine is compact and easy to use, making it perfect for various sealing tasks.', 130, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 10, N'2026-05-13 17:10:52.7277157', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (25, N'Packing Tape Dispenser, Durable Handheld Adhesive Tape Industrial Gun for Shipping, Moving, Carton and Box Sealing - " Wide Rolls', 0, 0, N'Packing Tape Dispenser, Durable Handheld Adhesive Tape Industrial Gun for Shipping, Moving, Carton and Box Sealing - " Wide Rolls', 400, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 10, N'2026-05-13 17:39:20.9301134', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (26, N'Mini Double Lens Fog Light Fleshing Light Reflection - Pack of 2 (Red & Blue)', 0, 0, N'Ultra Mini Double Lens Fog Light is designed for optimal visibility and safety on the road, featuring a sleek design that fits seamlessly with your bike. This pack includes two lights in vibrant red and blue, providing an effective solution for foggy conditions and enhancing your bike''s visibility.', 150, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 10, N'2026-05-13 17:44:56.2636844', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (27, N'Multicolor LED System Car', 0, 0, N'Multicolor LED System Car is an innovative LED lighting kit designed to enhance the appearance and visibility of your vehicle with customizable colors and remote control features.', 130, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 10, N'2026-05-13 17:46:06.319812', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (28, N'Trendy 4Pcs Bracelets Set for Women', 0, 0, N'Trendy 4Pcs Bracelets Set for Women is a fashionable and versatile jewelry collection designed to elevate your style effortlessly. This set features four unique bracelets that can be mixed and matched to create the perfect look for any occasion.', 45, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 6, N'2026-05-13 17:48:26.1297902', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (29, N'Bracelets 1Pc Trendy Loly Bracelet in Off White – Stylish Women''s Fashion Jewelry', 0, 0, N'1Pc Trendy Loly Bracelet Off White is a stylish and versatile accessory designed for women who appreciate fashion jewelry. Made from environmentally friendly materials, this bracelet is perfect for daily wear and gifting on special occasions.', 50, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 6, N'2026-05-13 17:49:10.7006544', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (30, N'Bracelets Elegant Necklace One Layer', 0, 0, N'Bracelets Elegant Necklace One Layer is the perfect accessory for women seeking a stylish yet casual look. This elegant necklace is part of the new collection designed specifically for women, making it an ideal choice for casual occasions.', 45, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 6, N'2026-05-13 17:50:19.0336356', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (31, N'Bracelets Elegant Necklace For Woman & Girls Two Layers', 0, 0, N'New Collection
Targeted Group : Women
Occasion : Casual
Type : One Layer
Color: May Vary
Doesn''t change color
ِِAway from the water', 45, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 6, N'2026-05-13 17:51:13.6690005', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (32, N'Honor X6c Dual SIM 4G Smartphone – 128GB Storage, 6GB RAM – Moonlight White', 0, 0, N'Body:

Dimensions: 164 × 75.6 × 8.4 mm
Weight: 199 g
SIM: Dual Nano-SIM
IP64 dust- and splash-resistant
Drop resistant up to 1.5 meters
Type: TFT LCD, 120Hz, 1010 nits peak brightness
Size: 6.61 inches (~84.9% screen-to-body ratio)
Resolution: 720 × 1604 pixels, 20:9 ratio (~266 ppi)
OS: Android 15, Magic OS 9
Chipset: Mediatek Helio G81 Ultra (12 nm)
CPU: Octa-core (2×2.0 GHz Cortex-A75 & 6×1.7 GHz Cortex-A55)
GPU: Mali-G52 MC2', 7290, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 4, N'2026-05-13 17:53:00.1581223', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (33, N'Samsung Galaxy A07 - 6.7" inch 256GB/8GB Dual SIM 4G Mobile Phone - Black', 0, 0, N'• Display: 6.7" PLS LCD, 90Hz, 720 x 1600 pixels
• Main Camera: 50 MP High-Resolution Dual Camera
• Battery: 5000 mAh with 25W Wired Charging
• Software Support: Up to 6 major Android upgrades
• Processor: Mediatek Helio G99 (6 nm)
• Design: 7.6 mm slim profile with IP54 dust and splash resistance
• Storage: 256GB 8GB RAM
• Security: Side-mounted fingerprint sensor
• Audio: 3.5mm headphone jack', 9790, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 4, N'2026-05-13 17:56:51.680714', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (34, N'Honor X5c Plus Dual SIM 4G 128GB/4GB - Ocean Cyan', 0, 0, N'Dimensions: 167 x 77 x 7.9 mm (6.57 x 3.03 x 0.31 in)
Weight: 186 g
Display Type: TFT LCD, 90Hz Refresh Rate
Size: 6.74 inches
Resolution: 720 x 1600 pixels (HD+)
Pixel Density: ~260 ppi density
OS: Android 15, MagicOS 9.0
Chipset: Mediatek Helio G81 (12 nm)
CPU: Octa-core (2x2.0 GHz Cortex-A75 & 6x1.7 GHz Cortex-A55)
GPU: Mali-G52 MC2
RAM:  4GB
Internal Storage:  128GB 
Card Slot: microSDXC (uses shared SIM slot - Hybrid)
Main Lens: 50 MP, f/1.8, (wide), PDAF
Auxiliary Lens: 0.08 MP QVGA (Depth/Auxiliary)
Features: LED flash, HDR, Panorama
Video: 1080p@30fps
Single Lens: 5 MP, f/2.2, (wide)
Video: 1080p@30fps
Type: Li-Po 5260 mAh (Typical) / 5130 mAh (Rated)
Charging: 15W Wired Fast Charging
Sensors: Fingerprint (side-mounted), Accelerometer, Proximity, Ambient Light
Colors: Ocean Cyan', 7040, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 4, N'2026-05-13 17:58:19.307711', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (35, N'OPPO RENO 15F 12GB/ 256GB 5G -AURORA BLUE', 0, 0, N'Key Features
 

Body:
Dimensions: 158.2 x 74.9 x 8.1 mm (6.23 x 2.95 x 0.32 in)
Weight: 195 g (6.88 oz)
Build: Glass front (Gorilla Glass+), aluminum frame, glass back (AGC Dragontrail DT-Star D+)
SIM: Nano-SIM
Water & Dust Resistance: IP68/IP69 (dust tight and water resistant; immersible up to 1.5m for 30 min, high-pressure water jets)
Display:
Type: AMOLED, 1B colors, 120Hz, 600 nits (typ), 1400 nits (HBM)
Size: 6.57 inches (~88.6% screen-to-body ratio)
Resolution: 1080 x 2372 pixels (~397 ppi density)
Protection: Corning Gorilla Glass+
Platform:
OS: Android 16, ColorOS 16
Chipset: Qualcomm SM6450 Snapdragon 6 Gen 1 (4 nm)
CPU: Octa-core (4x2.2 GHz Cortex-A78 & 4x1.8 GHz Cortex-A55)
GPU: Adreno 710
Memory:
Card Slot: microSDXC
Internal: 256GB 12GB RAM', 24150, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 4, N'2026-05-13 18:00:57.1206698', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (36, N'HP 14-em0025ne Laptop AMD Ryzen™ 5 7520U, 8GB Ram, 512GB SSD, AMD Radeon, 14.0 FHD, win 11 - Natural Silver', 0, 0, N'Brand: HP
Processor Type: AMD Ryzen 5 7520U
Gen: 7000 Series
Processor Core: Quad Core
Screen Size: 14 Inch
Resolution Type: FHD
Hard Disk Capacity: 512 GB SSD
RAM Capacity: 8 GB LPDDR5
Graphics Card Brand: AMD
Graphics Card: AMD AMD Radeon Graphics
Graphics Card Capacity: Built-In Graphics Card
Operating System: Win11 Home
Model Name: 14-em0025ne
Model Number: C53GREA
Color: Natural Silver
', 24869, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 5, N'2026-05-13 18:14:14.3297246', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (37, N'Lenovo LOQ Laptop, AMD Ryzen™ 7 250, 16GB Ram, 512GB SSD, NVIDIA® GeForce RTX™ 5050 8GB, 15.6" FHD, 83JG002BED -', 0, 0, N'Prosessor:AMD Ryzen™ 7 250
Graphics: NVIDIA® GeForce RTX™ 5050 8GB
Memory Ram: 16GB
Storage:512GB SSD
Color: Luna Grey
', 56639, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 5, N'2026-05-13 18:15:21.4447512', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (38, N'Lenovo IdeaPad Slim 3 15IRH10 ( Core i7-13620H-16G Ram DDR5-4800-512GB NVMe-15.3 Inch WUXGA,Luna Grey ) Lenovo WL310 Bluetooth Silent Mouse + Lenovo 16-inch Laptop Topload T210 Black (ECO) + 2 Year Warranty', 0, 0, N'processor:Intel Core i7-13620H
Memory :16G Ram
Storage :512GB NVMe
Display:15.3
 Warranty : 2-year', 41600, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 5, N'2026-05-13 18:18:37.6871332', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (39, N'Asus VivoBook Flip S14 Thin and Light 2-in-1 Laptop, 14 Inch Touch Display with Pen, Intel Core™ Ultra 7 256V, 16GB RAM, 512GB SSD, Intel Arc™ Graphics, Windows 11,TP3407SA-SG157W, Matte Gray', 0, 0, N'Processor	Intel® Core™ Ultra 7 Processor 256V 16GB 2.2 GHz (12MB Cache, up to 4.8 GHz, 8 cores, 8 Threads); Intel® AI Boost NPU up to 47TOPS
Memory	16GB LPDDR5X Memory on Package
Hard Drive	512GB M.2 NVMe™ PCIe® 3.0 SSD
Video Card	Intel® Arc™ Graphics
Display	14" WUXGA (1920 x 1200) OLED 16:10 aspect ratio ,60Hz refresh rate, 100% DCI-P3 color gamut
Operating System	Windows 11
Color	Matte Gray
WARRANTY	
1-Year ASUS Perfect warranty Pro

Activate your warranty here: https://eg.asus.click/pwe
Activation must be done within 90 days of purchase

Accessories	Stylus (ASUS Pen SA203H-MPP2.0 support)', 59945, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 5, N'2026-05-13 18:19:46.9519595', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (40, N'Portable 8 Pieces School Maths & Geometry Stationery Set for School and Office', 0, 0, N'Office Supplies, Promotion Gifts, School Supplies', 20, N'Like New', NULL, 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 9, N'2026-05-13 18:25:12.6123174', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (41, N'Premium OEM Office and School Stationery Supplies for Bulk Orders', 0, 0, N'Material:	Plastic
Ink Type:	Liquid-Ink', 20, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 9, N'2026-05-13 18:29:51.686452', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (42, N'Notebooks A4 Wholesale Stationery Notebook', 0, 0, N'Type:	Diary
Cover Material:	Linen', 20, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 9, N'2026-05-13 18:31:15.7895668', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (43, N'DOLLAR Pointer Plus BLACK Fineliner Fine Line Pens (10 x 0.3mm Fine Pens)', 0, 0, N'The pen is designed with a durable engineered 0.3mm tip to ensure smooth ink flow for smooth writing. The stylish and durable pen excellently glides on the paper for an incredibly neat writing experience.', 150, N'Like New', N'Cairo', 0, N'Sale', N'c474177c-799a-44b0-b74d-33f716a3a5fc', 9, N'2026-05-13 18:40:46.3431824', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (44, N'Straight Leg Jeans', 1, 1, N'Brand: Daly
Care instructions: Hand Wash Only
Brand: DALY
Care instructions: Hand Wash Only', 500, N'Like New', N'Cairo', 4, N'Sale', N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 3, N'2026-05-13 20:34:07.615772', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (45, N'SONY', 0, 0, N'ddd', 6666, N'Good', N'Asyut', 4, N'Sale', N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 1, N'2026-05-13 20:42:16.5204373', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (47, N'Samsung', 1, 1, N'w', 1000, N'Like New', N'Asyut', 5, N'Sale', N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 4, N'2026-05-14 02:06:16.1857913', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (48, N'Samsung', 1, 1, N'ddd', 1000, N'Like New', N'Asyut', 5, N'Sale', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 4, N'2026-05-14 02:12:18.2478986', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (49, N'SAM', 1, 1, N'GG', 666, N'Like New', N'Asyut', 4, N'Sale', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 4, N'2026-05-14 02:17:58.4210767', 0.0);
INSERT INTO [Items] ([ItemID], [Title], [IsAvailableForSale], [IsAvailableForSwap], [Description], [Price], [Condition], [Location], [Status], [ListingType], [UserID], [CategoryID], [CreatedAt], [PriceSortValue]) VALUES (50, N'Lmkm,', 1, 1, N'kjkjnk', 22, N'Very Good', N'Asyut', 4, N'Sale', N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 6, N'2026-05-14 15:01:50.7361353', 0.0);
GO
BEGIN TRY
    SET IDENTITY_INSERT [Items] OFF;
END TRY
BEGIN CATCH
END CATCH
GO
PRINT 'Importing BuyRequests...';
BEGIN TRY
    SET IDENTITY_INSERT [BuyRequests] ON;
END TRY
BEGIN CATCH
END CATCH
INSERT INTO [BuyRequests] ([BuyRequestId], [BuyerID], [SellerID], [ItemID], [Amount], [Status], [EscrowTransactionId], [CreatedAt], [UpdatedAt]) VALUES (1, N'ea6341a9-9414-4730-958f-e553691a55ee', N'0a76d56b-2546-4556-965d-633100430c75', 4, 800, 4, 3, N'2026-05-09 23:15:21.6807531', NULL);
INSERT INTO [BuyRequests] ([BuyRequestId], [BuyerID], [SellerID], [ItemID], [Amount], [Status], [EscrowTransactionId], [CreatedAt], [UpdatedAt]) VALUES (2, N'ea6341a9-9414-4730-958f-e553691a55ee', N'0a76d56b-2546-4556-965d-633100430c75', 4, 800, 4, 4, N'2026-05-12 20:20:23.4573526', NULL);
INSERT INTO [BuyRequests] ([BuyRequestId], [BuyerID], [SellerID], [ItemID], [Amount], [Status], [EscrowTransactionId], [CreatedAt], [UpdatedAt]) VALUES (3, N'ea6341a9-9414-4730-958f-e553691a55ee', N'0a76d56b-2546-4556-965d-633100430c75', 4, 800, 4, 5, N'2026-05-12 20:20:50.8678302', NULL);
INSERT INTO [BuyRequests] ([BuyRequestId], [BuyerID], [SellerID], [ItemID], [Amount], [Status], [EscrowTransactionId], [CreatedAt], [UpdatedAt]) VALUES (4, N'ea6341a9-9414-4730-958f-e553691a55ee', N'0a76d56b-2546-4556-965d-633100430c75', 4, 800, 4, 6, N'2026-05-12 20:25:09.0689266', NULL);
INSERT INTO [BuyRequests] ([BuyRequestId], [BuyerID], [SellerID], [ItemID], [Amount], [Status], [EscrowTransactionId], [CreatedAt], [UpdatedAt]) VALUES (5, N'ea6341a9-9414-4730-958f-e553691a55ee', N'0c4abf45-6a01-41c2-82b6-c3642e7b3cf6', 5, 500, 3, 8, N'2026-05-12 20:46:50.9388355', NULL);
INSERT INTO [BuyRequests] ([BuyRequestId], [BuyerID], [SellerID], [ItemID], [Amount], [Status], [EscrowTransactionId], [CreatedAt], [UpdatedAt]) VALUES (6, N'ea6341a9-9414-4730-958f-e553691a55ee', N'0c4abf45-6a01-41c2-82b6-c3642e7b3cf6', 5, 500, 4, 10, N'2026-05-12 20:58:28.2804083', NULL);
INSERT INTO [BuyRequests] ([BuyRequestId], [BuyerID], [SellerID], [ItemID], [Amount], [Status], [EscrowTransactionId], [CreatedAt], [UpdatedAt]) VALUES (7, N'ea6341a9-9414-4730-958f-e553691a55ee', N'0a76d56b-2546-4556-965d-633100430c75', 4, 800, 4, 11, N'2026-05-12 20:58:41.035822', NULL);
INSERT INTO [BuyRequests] ([BuyRequestId], [BuyerID], [SellerID], [ItemID], [Amount], [Status], [EscrowTransactionId], [CreatedAt], [UpdatedAt]) VALUES (8, N'ea6341a9-9414-4730-958f-e553691a55ee', N'0c4abf45-6a01-41c2-82b6-c3642e7b3cf6', 5, 500, 3, 13, N'2026-05-12 21:23:59.9664292', NULL);
INSERT INTO [BuyRequests] ([BuyRequestId], [BuyerID], [SellerID], [ItemID], [Amount], [Status], [EscrowTransactionId], [CreatedAt], [UpdatedAt]) VALUES (9, N'ea6341a9-9414-4730-958f-e553691a55ee', N'0c4abf45-6a01-41c2-82b6-c3642e7b3cf6', 5, 500, 3, 14, N'2026-05-12 21:34:16.7503791', NULL);
INSERT INTO [BuyRequests] ([BuyRequestId], [BuyerID], [SellerID], [ItemID], [Amount], [Status], [EscrowTransactionId], [CreatedAt], [UpdatedAt]) VALUES (10, N'ea6341a9-9414-4730-958f-e553691a55ee', N'0c4abf45-6a01-41c2-82b6-c3642e7b3cf6', 5, 500, 2, 15, N'2026-05-12 21:45:48.757593', NULL);
INSERT INTO [BuyRequests] ([BuyRequestId], [BuyerID], [SellerID], [ItemID], [Amount], [Status], [EscrowTransactionId], [CreatedAt], [UpdatedAt]) VALUES (11, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 44, 500, 3, 20, N'2026-05-13 20:36:00.2831666', NULL);
INSERT INTO [BuyRequests] ([BuyRequestId], [BuyerID], [SellerID], [ItemID], [Amount], [Status], [EscrowTransactionId], [CreatedAt], [UpdatedAt]) VALUES (12, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 44, 500, 2, 21, N'2026-05-13 20:38:05.9904901', NULL);
INSERT INTO [BuyRequests] ([BuyRequestId], [BuyerID], [SellerID], [ItemID], [Amount], [Status], [EscrowTransactionId], [CreatedAt], [UpdatedAt]) VALUES (13, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 45, 6666, 2, 25, N'2026-05-13 20:42:59.9149196', NULL);
INSERT INTO [BuyRequests] ([BuyRequestId], [BuyerID], [SellerID], [ItemID], [Amount], [Status], [EscrowTransactionId], [CreatedAt], [UpdatedAt]) VALUES (14, N'da368678-4a4a-4b5c-bd6e-993c0834dbb7', N'0a76d56b-2546-4556-965d-633100430c75', 4, 800, 1, 30, N'2026-05-13 21:25:06.5463274', NULL);
INSERT INTO [BuyRequests] ([BuyRequestId], [BuyerID], [SellerID], [ItemID], [Amount], [Status], [EscrowTransactionId], [CreatedAt], [UpdatedAt]) VALUES (15, N'd1bbac46-7d40-4800-8c4a-a560cb499c5f', N'0a76d56b-2546-4556-965d-633100430c75', 3, 1000, 2, 33, N'2026-05-13 23:54:06.1925893', NULL);
INSERT INTO [BuyRequests] ([BuyRequestId], [BuyerID], [SellerID], [ItemID], [Amount], [Status], [EscrowTransactionId], [CreatedAt], [UpdatedAt]) VALUES (16, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 49, 666, 2, 38, N'2026-05-14 02:18:17.8321766', NULL);
INSERT INTO [BuyRequests] ([BuyRequestId], [BuyerID], [SellerID], [ItemID], [Amount], [Status], [EscrowTransactionId], [CreatedAt], [UpdatedAt]) VALUES (17, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 50, 22, 2, 41, N'2026-05-14 15:03:48.0460352', NULL);
GO
BEGIN TRY
    SET IDENTITY_INSERT [BuyRequests] OFF;
END TRY
BEGIN CATCH
END CATCH
GO
PRINT 'Importing Conversations...';
BEGIN TRY
    SET IDENTITY_INSERT [Conversations] ON;
END TRY
BEGIN CATCH
END CATCH
INSERT INTO [Conversations] ([ConversationID], [ItemID], [BuyerID], [SellerID], [CreatedAt]) VALUES (1, 44, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', N'2026-05-13 20:34:54.7414899');
INSERT INTO [Conversations] ([ConversationID], [ItemID], [BuyerID], [SellerID], [CreatedAt]) VALUES (2, 3, N'd1bbac46-7d40-4800-8c4a-a560cb499c5f', N'0a76d56b-2546-4556-965d-633100430c75', N'2026-05-13 23:53:19.1223353');
INSERT INTO [Conversations] ([ConversationID], [ItemID], [BuyerID], [SellerID], [CreatedAt]) VALUES (3, 50, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'2026-05-14 15:03:31.172887');
GO
BEGIN TRY
    SET IDENTITY_INSERT [Conversations] OFF;
END TRY
BEGIN CATCH
END CATCH
GO
PRINT 'Importing Favorites...';
BEGIN TRY
    SET IDENTITY_INSERT [Favorites] ON;
END TRY
BEGIN CATCH
END CATCH
INSERT INTO [Favorites] ([FavoriteID], [UserID], [ItemID], [CreatedAt], [ItemID1]) VALUES (1, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 42, N'2026-05-13 17:32:17.810237', NULL);
INSERT INTO [Favorites] ([FavoriteID], [UserID], [ItemID], [CreatedAt], [ItemID1]) VALUES (2, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 43, N'2026-05-13 17:32:19.8701469', NULL);
INSERT INTO [Favorites] ([FavoriteID], [UserID], [ItemID], [CreatedAt], [ItemID1]) VALUES (3, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 44, N'2026-05-13 17:35:40.8311356', NULL);
INSERT INTO [Favorites] ([FavoriteID], [UserID], [ItemID], [CreatedAt], [ItemID1]) VALUES (4, N'd1bbac46-7d40-4800-8c4a-a560cb499c5f', 43, N'2026-05-13 20:52:28.5867014', NULL);
INSERT INTO [Favorites] ([FavoriteID], [UserID], [ItemID], [CreatedAt], [ItemID1]) VALUES (5, N'd1bbac46-7d40-4800-8c4a-a560cb499c5f', 42, N'2026-05-13 20:52:30.1953398', NULL);
INSERT INTO [Favorites] ([FavoriteID], [UserID], [ItemID], [CreatedAt], [ItemID1]) VALUES (6, N'd1bbac46-7d40-4800-8c4a-a560cb499c5f', 41, N'2026-05-13 20:52:31.7237003', NULL);
GO
BEGIN TRY
    SET IDENTITY_INSERT [Favorites] OFF;
END TRY
BEGIN CATCH
END CATCH
GO
PRINT 'Importing ItemImages...';
BEGIN TRY
    SET IDENTITY_INSERT [ItemImages] ON;
END TRY
BEGIN CATCH
END CATCH
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (1, N'/uploads/97857c82-c3fb-4ac8-99be-7a089f4b9e3f_flash.jpeg', 1);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (2, N'/uploads/276f0819-856f-4c9d-8984-bc7672720abe_power.jpeg', 2);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (3, N'/uploads/0a3f35cc-4345-4d0b-b98c-b8ef18fe3b40_WhatsApp Image 2026-05-09 at 9.38.36 PM.jpeg', 3);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (4, N'/uploads/1238385c-da5c-4a62-9596-3647729d5e94_WhatsApp Image 2026-05-09 at 9.39.36 PM.jpeg', 4);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (5, N'/uploads/1b3ec341-79cd-4bb9-abbe-2a75b8629d49_photo_2026-02-21_03-34-53.jpg', 5);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (8, N'/uploads/6ff58808-12fc-4b2a-a288-852d385995ab_images.jpg', 8);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (9, N'/uploads/e68e4daf-b365-450c-a9eb-ecae12ca93d7_download.jpg', 9);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (10, N'/uploads/3a232743-455f-43b0-89c6-e2a31bdd9292_1.jpg', 10);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (11, N'/uploads/d2b58b2f-cb21-45c1-b9af-04e9d7d54733_1.jpg', 11);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (12, N'/uploads/c4c411a9-a93b-4268-bcd9-2a91cfb9e21d_1.jpg', 12);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (13, N'/uploads/b6ddeb31-6637-485f-b82b-2fe628a7f2ec_1.jpg', 13);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (14, N'/uploads/88083bba-e484-4d2d-89df-715a78606f1f_1.jpg', 14);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (15, N'/uploads/13a3f073-4d66-4165-a535-957299756e72_1.jpg', 15);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (17, N'/uploads/773cd111-82f0-4198-a3f9-0687004707bc_1.jpg', 17);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (18, N'/uploads/5768db12-63a4-477e-88ae-46d4f62e8700_1.jpg', 18);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (19, N'/uploads/d611f27a-3f71-4994-bbb1-0fd2f5bf8d04_1.jpg', 19);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (20, N'/uploads/07c18fd0-d76b-4c58-86a6-05aa9edad190_1.jpg', 20);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (21, N'/uploads/033badec-4590-440b-9fbd-f7d26fbdd8d2_1.jpg', 21);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (22, N'/uploads/a7f844f5-f850-4a2b-8acb-c6ff098d6b6e_1.jpg', 22);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (23, N'/uploads/beecf531-b078-46d4-ae26-b0988ea82cbf_1.jpg', 23);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (24, N'/uploads/f7ddbe00-ac20-40f4-8663-6d2c20314e76_1.jpg', 24);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (25, N'/uploads/1c411f91-729d-4574-a7aa-f52676c88457_1.jpg', 25);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (26, N'/uploads/21a6f467-5c16-4e9a-ac9f-d4dabfe1d104_1.jpg', 26);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (27, N'/uploads/952895ea-4afc-4faa-bc9b-95e6d00bbd9a_1.jpg', 27);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (28, N'/uploads/a01cba79-375f-40ab-b328-62b4dd591c91_1.jpg', 28);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (29, N'/uploads/2f52c83d-25e3-4dc2-8a5e-3302c3dc926f_1.jpg', 29);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (30, N'/uploads/7333d2b2-f2e8-4f13-9e4e-c90c9120c0b4_1.jpg', 30);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (31, N'/uploads/dbce2848-bfbd-465b-a25b-b9484e90fff4_1.jpg', 31);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (32, N'/uploads/1e774528-2378-4baa-9526-8988a1857171_1.jpg', 32);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (33, N'/uploads/53a5e145-4d31-4129-9f4b-cc61eeb05a01_1.jpg', 33);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (34, N'/uploads/50bdefc2-21d8-495b-87b6-04f2fd3d39bb_1.jpg', 34);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (35, N'/uploads/ca311124-4ee4-4b20-9ef8-1544c9269d7c_1.jpg', 35);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (36, N'/uploads/b945b991-645b-48a3-ae71-32a369c7dea3_1.jpg', 36);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (37, N'/uploads/f5d3a2dd-43a3-4534-9516-279c10f7e3bf_1.jpg', 37);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (38, N'/uploads/44c350e1-ee61-4e2f-850a-400be8c06bdf_1.jpg', 38);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (39, N'/uploads/ea015cb1-1bed-4825-b2ea-cbe3f80f6a65_1.jpg', 39);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (40, N'/uploads/0c6b6285-d8ad-4bff-b31d-302849f3780a_Portable-8-Pieces-School-Maths-Geometry-Stationery-Set-for-School-and-Office.webp', 40);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (41, N'/uploads/c5809c37-ed52-42bb-94fe-059fbf201839_Premium-OEM-Office-and-School-Stationery-Supplies-for-Bulk-Orders.webp', 41);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (42, N'/uploads/85d39b66-9d15-4b72-8458-d40e6dcd2e34_Children-S-School-Supplies-and-Notebooks-A4-Wholesale-Stationery-Notebook-13532-.webp', 42);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (43, N'/uploads/223ae89f-bdd0-46f9-bb17-532c86ab20df_91W4Ytx-15L._AC_SL1500_.jpg', 43);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (44, N'/uploads/d9152229-3a13-4434-bcf3-31802c58f8c7_61pdBxJRLXL._AC_SY741_.jpg', 44);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (45, N'/uploads/66be06e0-2a30-47d7-aa9f-aac25127546a_71ybmzfTkHL._AC_SL1500_.jpg', 45);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (47, N'/uploads/bd3819fe-23c4-4181-985b-7af98842f598_71ybmzfTkHL._AC_SL1500_.jpg', 47);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (48, N'/uploads/a0e29b0f-2c5e-452f-8bd6-0a67b88ecf0f_71ybmzfTkHL._AC_SL1500_.jpg', 48);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (49, N'/uploads/26e6081c-8b5c-4eee-89e2-6f882ee54dd8_71ybmzfTkHL._AC_SL1500_.jpg', 49);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (50, N'/uploads/5550b5ea-8902-49d3-8d3b-71d8b88914ca_WhatsApp Image 2026-05-11 at 6.15.56 PM.jpeg', 50);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (51, N'/uploads/8aeb903b-135b-430f-b3c1-7ab4672738f5_WhatsApp Image 2026-05-11 at 6.19.44 PM.jpeg', 50);
INSERT INTO [ItemImages] ([ImageID], [ImageUrl], [ItemID]) VALUES (52, N'/uploads/d58b24b3-8d3f-4d3c-8aef-d1a53b97a450_WhatsApp Image 2026-05-12 at 7.37.58 PM.jpeg', 50);
GO
BEGIN TRY
    SET IDENTITY_INSERT [ItemImages] OFF;
END TRY
BEGIN CATCH
END CATCH
GO
PRINT 'Importing SwapRequests...';
BEGIN TRY
    SET IDENTITY_INSERT [SwapRequests] ON;
END TRY
BEGIN CATCH
END CATCH
INSERT INTO [SwapRequests] ([SwapRequestId], [RequesterId], [OfferedItemId], [RequestedItemId], [Status], [CreatedAt], [ApplicationUserId]) VALUES (1, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 48, 47, 1, N'2026-05-14 02:12:32.2478176', NULL);
GO
BEGIN TRY
    SET IDENTITY_INSERT [SwapRequests] OFF;
END TRY
BEGIN CATCH
END CATCH
GO
PRINT 'Importing Transactions...';
BEGIN TRY
    SET IDENTITY_INSERT [Transactions] ON;
END TRY
BEGIN CATCH
END CATCH
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (1, NULL, N'ea6341a9-9414-4730-958f-e553691a55ee', NULL, 100, 2, 2, 3, N'Top-up: $100.00', N'2026-05-09 23:10:59.6599621');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (2, NULL, N'ea6341a9-9414-4730-958f-e553691a55ee', NULL, 800, 2, 2, 3, N'Top-up: $800.00', N'2026-05-09 23:11:14.975313');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (3, 4, N'ea6341a9-9414-4730-958f-e553691a55ee', N'0a76d56b-2546-4556-965d-633100430c75', 800, 0, 2, 0, N'Escrow Hold for: Tapo c200 360-degree smart wi-fi pan and tilt camera, 1080 p - white', N'2026-05-09 23:15:21.6235153');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (7, NULL, N'ea6341a9-9414-4730-958f-e553691a55ee', NULL, 500, 2, 2, 3, N'Top-up: $500.00', N'2026-05-12 20:45:28.1141742');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (8, 5, N'ea6341a9-9414-4730-958f-e553691a55ee', N'0c4abf45-6a01-41c2-82b6-c3642e7b3cf6', 500, 0, 2, 0, N'Escrow Hold for: AA', N'2026-05-12 20:46:50.8498905');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (9, NULL, N'ea6341a9-9414-4730-958f-e553691a55ee', NULL, 100, 2, 2, 3, N'Top-up: $100.00', N'2026-05-12 20:47:41.2031767');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (12, NULL, N'ea6341a9-9414-4730-958f-e553691a55ee', N'ea6341a9-9414-4730-958f-e553691a55ee', 250, 2, 2, 3, N'Top-up Balance: $250.00', N'2026-05-12 20:59:05.2044841');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (14, 5, N'ea6341a9-9414-4730-958f-e553691a55ee', N'0c4abf45-6a01-41c2-82b6-c3642e7b3cf6', 500, 3, 2, 0, N'Order rejected by seller. Funds refunded.', N'2026-05-12 21:34:16.1613098');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (15, 5, N'ea6341a9-9414-4730-958f-e553691a55ee', N'0c4abf45-6a01-41c2-82b6-c3642e7b3cf6', 500, 0, 2, 0, N'Escrow Hold for: AA', N'2026-05-12 21:45:48.2763928');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (16, 5, N'ea6341a9-9414-4730-958f-e553691a55ee', N'0c4abf45-6a01-41c2-82b6-c3642e7b3cf6', 250, 1, 2, 1, NULL, N'2026-05-12 21:46:14.2641677');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (17, 5, N'ea6341a9-9414-4730-958f-e553691a55ee', N'0c4abf45-6a01-41c2-82b6-c3642e7b3cf6', 250, 2, 2, 2, NULL, N'2026-05-12 21:47:35.2656656');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (18, NULL, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 9999.98, 2, 2, 3, N'Wallet Deposit', N'2026-05-13 20:29:33.0001595');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (19, NULL, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 500, 2, 2, 3, N'Wallet Deposit', N'2026-05-13 20:35:52.3696026');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (20, 44, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 500, 3, 2, 0, N'Order rejected by seller. Funds refunded.', N'2026-05-13 20:36:00.2737635');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (21, 44, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 500, 0, 2, 0, N'Escrow Hold for: Straight Leg Jeans', N'2026-05-13 20:38:05.9819096');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (22, 44, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 250, 1, 2, 1, NULL, N'2026-05-13 20:38:55.9017142');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (23, 44, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 250, 2, 2, 2, NULL, N'2026-05-13 20:39:30.9902687');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (24, NULL, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 9999.99, 2, 2, 3, N'Wallet Deposit', N'2026-05-13 20:42:50.5988376');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (25, 45, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 6666, 0, 2, 0, N'Escrow Hold for: SONY', N'2026-05-13 20:42:59.9046748');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (26, 45, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 3333, 1, 2, 1, NULL, N'2026-05-13 20:43:24.7431631');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (27, 45, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 3333, 2, 2, 2, NULL, N'2026-05-13 20:43:47.1143754');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (28, NULL, N'da368678-4a4a-4b5c-bd6e-993c0834dbb7', N'da368678-4a4a-4b5c-bd6e-993c0834dbb7', 500, 2, 2, 3, N'Wallet Deposit', N'2026-05-13 21:24:13.703171');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (29, NULL, N'da368678-4a4a-4b5c-bd6e-993c0834dbb7', N'da368678-4a4a-4b5c-bd6e-993c0834dbb7', 500, 2, 2, 3, N'Wallet Deposit', N'2026-05-13 21:24:45.3755735');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (30, 4, N'da368678-4a4a-4b5c-bd6e-993c0834dbb7', N'0a76d56b-2546-4556-965d-633100430c75', 800, 0, 2, 0, N'Escrow Hold for: Tapo c200 360-degree smart wi-fi pan and tilt camera, 1080 p - white', N'2026-05-13 21:25:06.5143317');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (31, 4, N'da368678-4a4a-4b5c-bd6e-993c0834dbb7', N'0a76d56b-2546-4556-965d-633100430c75', 400, 1, 2, 1, NULL, N'2026-05-13 21:25:37.2404709');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (32, NULL, N'd1bbac46-7d40-4800-8c4a-a560cb499c5f', N'd1bbac46-7d40-4800-8c4a-a560cb499c5f', 10000, 2, 2, 3, N'Wallet Deposit', N'2026-05-13 23:53:48.7073639');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (33, 3, N'd1bbac46-7d40-4800-8c4a-a560cb499c5f', N'0a76d56b-2546-4556-965d-633100430c75', 1000, 0, 2, 0, N'Escrow Hold for: Brand : Logitech Connectivity : USB Device Type : Over-Ear Headphones Part Number : 981-000406 Color : Black', N'2026-05-13 23:54:06.1802492');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (34, 3, N'd1bbac46-7d40-4800-8c4a-a560cb499c5f', N'0a76d56b-2546-4556-965d-633100430c75', 500, 1, 2, 1, NULL, N'2026-05-13 23:54:57.2940991');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (35, 3, N'd1bbac46-7d40-4800-8c4a-a560cb499c5f', N'0a76d56b-2546-4556-965d-633100430c75', 500, 2, 2, 2, NULL, N'2026-05-13 23:56:14.579307');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (36, 48, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 1000, 2, 2, 5, N'Swap: Gave "Samsung" for "Samsung"', N'2026-05-14 02:13:16.0668164');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (37, 47, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 1000, 2, 2, 5, N'Swap: Gave "Samsung" for "Samsung"', N'2026-05-14 02:13:16.0710767');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (38, 49, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 666, 0, 2, 0, N'Escrow Hold for: SAM', N'2026-05-14 02:18:17.7741466');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (39, 49, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 333, 1, 2, 1, NULL, N'2026-05-14 02:18:46.3946012');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (40, 49, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 333, 2, 2, 2, NULL, N'2026-05-14 02:19:19.0386853');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (41, 50, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 22, 0, 2, 0, N'Escrow Hold for: Lmkm,', N'2026-05-14 15:03:47.9786188');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (42, 50, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 11, 1, 2, 1, NULL, N'2026-05-14 15:05:20.8373367');
INSERT INTO [Transactions] ([TransactionID], [ItemID], [BuyerID], [SellerID], [FinalPrice], [Status], [PaymentMethod], [Type], [Notes], [CreatedAt]) VALUES (43, 50, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 11, 2, 2, 2, NULL, N'2026-05-14 15:06:22.9380277');
GO
BEGIN TRY
    SET IDENTITY_INSERT [Transactions] OFF;
END TRY
BEGIN CATCH
END CATCH
GO
PRINT 'Importing Messages...';
BEGIN TRY
    SET IDENTITY_INSERT [Messages] ON;
END TRY
BEGIN CATCH
END CATCH
INSERT INTO [Messages] ([MessageID], [ConversationID], [SenderID], [MessageText], [CreatedAt], [IsRead]) VALUES (1, 1, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'hey arwa i wanna buy this pants', N'2026-05-13 20:35:24.4398226', 0);
INSERT INTO [Messages] ([MessageID], [ConversationID], [SenderID], [MessageText], [CreatedAt], [IsRead]) VALUES (2, 1, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', N'hey how are?', N'2026-05-13 20:37:01.5267702', 0);
INSERT INTO [Messages] ([MessageID], [ConversationID], [SenderID], [MessageText], [CreatedAt], [IsRead]) VALUES (3, 2, N'd1bbac46-7d40-4800-8c4a-a560cb499c5f', N'hi ', N'2026-05-13 23:53:23.1428403', 0);
INSERT INTO [Messages] ([MessageID], [ConversationID], [SenderID], [MessageText], [CreatedAt], [IsRead]) VALUES (4, 2, N'0a76d56b-2546-4556-965d-633100430c75', N'hello', N'2026-05-13 23:58:25.8910325', 0);
INSERT INTO [Messages] ([MessageID], [ConversationID], [SenderID], [MessageText], [CreatedAt], [IsRead]) VALUES (5, 3, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', N'khkjhk', N'2026-05-14 15:03:36.8021095', 0);
GO
BEGIN TRY
    SET IDENTITY_INSERT [Messages] OFF;
END TRY
BEGIN CATCH
END CATCH
GO
PRINT 'Importing Reviews...';
BEGIN TRY
    SET IDENTITY_INSERT [Reviews] ON;
END TRY
BEGIN CATCH
END CATCH
INSERT INTO [Reviews] ([ReviewID], [TransactionID], [ReviewerID], [SellerID], [Rating], [Comment], [CreatedAt]) VALUES (1, 23, N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', 4, N'', N'2026-05-13 20:39:36.4057602');
INSERT INTO [Reviews] ([ReviewID], [TransactionID], [ReviewerID], [SellerID], [Rating], [Comment], [CreatedAt]) VALUES (3, 43, N'b0ba1da7-97fc-4b54-9ee4-eb69add38324', N'73b42d7c-73bc-4cbe-9001-fc1c8de1254f', 4, N'', N'2026-05-14 15:06:32.0182357');
GO
BEGIN TRY
    SET IDENTITY_INSERT [Reviews] OFF;
END TRY
BEGIN CATCH
END CATCH
GO
EXEC sp_MSforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all";
GO
