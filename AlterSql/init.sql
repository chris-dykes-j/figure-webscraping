DROP TABLE IF EXISTS languages, figure, figure_name, series_name;

CREATE TABLE languages (
  language_code CHAR(2) PRIMARY KEY, 
  language_name VARCHAR(255) NOT NULL
);

INSERT INTO languages (language_code, language_name)
VALUES ('en', 'English'), ('ja', 'Japanese');

CREATE TABLE figure
(
  id SERIAL PRIMARY KEY,
  scale VARCHAR(4),
  brand VARCHAR(255),
  origin_url VARCHAR(255) NOT NULL
);

CREATE TABLE figure_name
(
  figure_id INT NOT NULL,
  language_code CHAR(2) NOT NULL,
  text VARCHAR(255) NOT NULL,
  PRIMARY KEY (figure_id, language_code),
  FOREIGN KEY (figure_id) REFERENCES figure (id),
  FOREIGN KEY (language_code) REFERENCES languages (language_code)
);

CREATE TABLE series_name
(
  figure_id INT NOT NULL,
  language_code CHAR(2) NOT NULL,
  text VARCHAR(255) NOT NULL,
  PRIMARY KEY (figure_id, language_code),
  FOREIGN KEY (figure_id) REFERENCES figure (id),
  FOREIGN KEY (language_code) REFERENCES languages (language_code)
);

CREATE TABLE character_name
(
  id SERIAL PRIMARY KEY,
  figure_id INT NOT NULL,
  language_code CHAR(2) NOT NULL,
  text VARCHAR(255) NOT NULL,
  UNIQUE (figure_id, language_code, text),
  FOREIGN KEY (figure_id) REFERENCES figure (id),
  FOREIGN KEY (language_code) REFERENCES languages (language_code)
);

CREATE TABLE sculptor
(
  id SERIAL PRIMARY KEY,
  figure_id INT NOT NULL,
  language_code CHAR(2) NOT NULL,
  text VARCHAR(255) NOT NULL,
  UNIQUE (figure_id, language_code, text),
  FOREIGN KEY (figure_id) REFERENCES figure (id),
  FOREIGN KEY (language_code) REFERENCES languages (language_code)
);

CREATE TABLE painter
(
  id SERIAL PRIMARY KEY,
  figure_id INT NOT NULL,
  language_code CHAR(2) NOT NULL,
  text VARCHAR(255) NOT NULL,
  UNIQUE (figure_id, language_code, text),
  FOREIGN KEY (figure_id) REFERENCES figure (id),
  FOREIGN KEY (language_code) REFERENCES languages (language_code)
);

CREATE TABLE material 
(
  id SERIAL PRIMARY KEY,
  figure_id INT NOT NULL,
  language_code CHAR(2) NOT NULL,
  text VARCHAR(255) NOT NULL,
  UNIQUE (figure_id, language_code, text),
  FOREIGN KEY (figure_id) REFERENCES figure (id),
  FOREIGN KEY (language_code) REFERENCES languages (language_code)
);

CREATE TABLE measurement
(
  id SERIAL PRIMARY KEY,
  figure_id INT NOT NULL,
  language_code CHAR(2) NOT NULL,
  text VARCHAR(255) NOT NULL,
  UNIQUE (figure_id, language_code, text),
  FOREIGN KEY (figure_id) REFERENCES figure (id),
  FOREIGN KEY (language_code) REFERENCES languages (language_code)
);

CREATE TABLE release_date
(
  id SERIAL PRIMARY KEY,
  figure_id INT NOT NULL,
  release_year INTEGER NOT NULL,
  release_month SMALLINT NOT NULL CHECK (release_month BETWEEN 1 AND 12),
  FOREIGN KEY (figure_id) REFERENCES figure(id)
);

CREATE TABLE price
(
  id SERIAL PRIMARY KEY,
  figure_id INT NOT NULL,
  price_with_tax INTEGER,
  price_without_tax INTEGER,
  currency CHAR(3) NOT NULL DEFAULT 'JPY',
  FOREIGN KEY (figure_id) REFERENCES figure(id)
);

CREATE TABLE blog_url
(
  id SERIAL PRIMARY KEY,
  figure_id INT NOT NULL,
  blog_url VARCHAR(255) NOT NULL UNIQUE,
  FOREIGN KEY (figure_id) REFERENCES figure(id)
);