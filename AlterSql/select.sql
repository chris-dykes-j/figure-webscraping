SELECT
  f.*,
  fn.text AS figure_name,
  sn.text AS series_name,
  cn.text AS character_name,
  bu.blog_url AS blog_url,
  p.price_with_tax,
  p.price_without_tax,
  p.currency
FROM
  public.figure AS f
    LEFT JOIN
  public.figure_name AS fn ON f.id = fn.figure_id
    LEFT JOIN
  public.series_name AS sn ON f.id = sn.figure_id
    LEFT JOIN
  public.character_name AS cn ON f.id = cn.figure_id
    LEFT JOIN
  public.blog_url AS bu on f.id = bu.figure_id
    LEFT JOIN
  public.price AS p on f.id = p.figure_id;
