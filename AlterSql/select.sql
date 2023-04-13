SELECT
  f.*,
  fn.text AS figure_name,
  sn.text AS series_name
FROM
  public.figure AS f
    JOIN
  public.figure_name AS fn ON f.id = fn.figure_id
    JOIN
  public.series_name AS sn ON f.id = sn.figure_id;