SELECT
  f.id,
  f.scale,
  f.brand,
  f.origin_url,
  fn.text AS figure_name,
  sn.text AS series_name,
  cn.text AS character_name,
  s.text AS sculptor,
  p.text AS painter,
  m.material,
  me.text AS measurement,
  rd.release_year,
  rd.release_month,
  pr.price_with_tax,
  pr.price_without_tax,
  pr.currency,
  pr.edition,
  bu.blog_url
FROM
  figure AS f
    LEFT JOIN figure_name AS fn ON f.id = fn.figure_id AND fn.language_code = 'ja'
    LEFT JOIN series_name AS sn ON f.id = sn.figure_id AND sn.language_code = 'ja'
    LEFT JOIN character_name AS cn ON f.id = cn.figure_id AND cn.language_code = 'ja'
    LEFT JOIN sculptor AS s ON f.id = s.figure_id AND s.language_code = 'ja'
    LEFT JOIN painter AS p ON f.id = p.figure_id AND p.language_code = 'ja'
    LEFT JOIN material AS m ON f.id = m.figure_id
    LEFT JOIN measurement AS me ON f.id = me.figure_id AND me.language_code = 'ja'
    LEFT JOIN release_date AS rd ON f.id = rd.figure_id
    LEFT JOIN price AS pr ON f.id = pr.figure_id
    LEFT JOIN blog_url AS bu ON f.id = bu.figure_id;
