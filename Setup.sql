--  CREATE TABLE accounts (
--   id VARCHAR (255) NOT NULL,
--   name VARCHAR (255) NOT NULL,
--   email VARCHAR (255) NOT NULL,
--   picture VARCHAR (255),


--   PRIMARY KEY (id)
-- ); 


--  CREATE TABLE blogs (
--   id INT NOT NULL AUTO_INCREMENT,
--   creatorId VARCHAR (255) NOT NULL,
--   title VARCHAR (255) NOT NULL,
--   body VARCHAR (255) NOT NULL,
--   imgUrl VARCHAR (255),
--   published BOOLEAN,


--   PRIMARY KEY (id),

--   FOREIGN KEY (creatorId)
--   REFERENCES accounts (id)
--   ON DELETE CASCADE

-- ); 


--  CREATE TABLE comments (
--   id INT NOT NULL AUTO_INCREMENT,
--   creatorId VARCHAR (255) NOT NULL,
--   body VARCHAR (255) NOT NULL,
--   blogId INT NOT NULL,


--   PRIMARY KEY (id),

--   FOREIGN KEY (creatorId)
--   REFERENCES accounts (id)
--   ON DELETE CASCADE,

--   FOREIGN KEY (blogId)
--   REFERENCES blogs (id)
--   ON DELETE CASCADE
-- ); 





/* !!!! DANGER ZONE !!!! */
--  DELETE ENTIRE TABLE 
--  DROP TABLE blogs 