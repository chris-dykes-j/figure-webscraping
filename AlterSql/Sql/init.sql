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
  id SERIAL PRIMARY KEY,
  figure_id INT NOT NULL,
  language_code CHAR(2) NOT NULL,
  text VARCHAR(255) NOT NULL,
  FOREIGN KEY (figure_id) REFERENCES figure (id),
  FOREIGN KEY (language_code) REFERENCES languages (language_code)
  --UNIQUE (figure_id, language_code)
);

CREATE TABLE series_name
(
  id SERIAL PRIMARY KEY,
  figure_id INT NOT NULL,
  language_code CHAR(2) NOT NULL,
  text VARCHAR(255) NOT NULL,
  FOREIGN KEY (figure_id) REFERENCES figure (id),
  FOREIGN KEY (language_code) REFERENCES languages (language_code)
  --UNIQUE (figure_id, language_code)
);
