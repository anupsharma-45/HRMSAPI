-- 1. Seed Initial Roles
INSERT INTO Roles (Id, Name, Description, CreatedAt, UpdatedAt)
VALUES 
(NEWID(), 'SuperAdmin', 'Overall system administrator', GETUTCDATE(), GETUTCDATE()),
(NEWID(), 'OrgAdmin', 'Organization level administrator', GETUTCDATE(), GETUTCDATE()),
(NEWID(), 'Employee', 'Regular employee', GETUTCDATE(), GETUTCDATE());

-- 2. Seed Initial Permissions
INSERT INTO Permissions (Id, Name, Code, Module, CreatedAt, UpdatedAt)
VALUES 
(NEWID(), 'Create User', 'USER_CREATE', 'UserManagement', GETUTCDATE(), GETUTCDATE()),
(NEWID(), 'Edit User', 'USER_EDIT', 'UserManagement', GETUTCDATE(), GETUTCDATE()),
(NEWID(), 'Delete User', 'USER_DELETE', 'UserManagement', GETUTCDATE(), GETUTCDATE()),
(NEWID(), 'View Dashboard', 'DASHBOARD_VIEW', 'Dashboard', GETUTCDATE(), GETUTCDATE());

-- 3. Seed a Sample User (Password is 'Admin@123')
-- Note: 'Password' column matches the mapping in AppDbContext for PasswordHash property
-- Note: 'PhoneNumber' column matches the mapping in AppDbContext for Phone property
INSERT INTO Users (Id, FirstName, LastName, Email, Password, PhoneNumber, IsActive, IsDeleted, CreatedAt, UpdatedAt)
VALUES 
(NEWID(), 'Admin', 'User', 'admin@hrms.com', '$2a$11$G7V4q8O3z6/yL/V6eO2/6uX1y0.xG5r1/fFz9vH9.rP.yY.zX.y6', '1234567890', 1, 0, GETUTCDATE(), GETUTCDATE());

-- 4. Link Admin to SuperAdmin role
INSERT INTO UserRoles (Id, UserId, RoleId)
SELECT NEWID(), u.Id, r.Id FROM Users u, Roles r WHERE u.Email = 'admin@hrms.com' AND r.Name = 'SuperAdmin';