import * as React from "react";
import TextField from "@mui/material/TextField";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import FormControl from "@mui/material/FormControl";
import Select from "@mui/material/Select";
import FormLabel from "@mui/material/FormLabel";
import FormGroup from "@mui/material/FormGroup";
import FormControlLabel from "@mui/material/FormControlLabel";
import Checkbox from "@mui/material/Checkbox";
import Button from "@mui/material/Button";
import SendIcon from "@mui/icons-material/Send";
import { useState, useRef, useEffect } from "react";

import { styled } from "@mui/material/styles";
import Card from "@mui/material/Card";
import CardMedia from "@mui/material/CardMedia";
import CardContent from "@mui/material/CardContent";
import CardActions from "@mui/material/CardActions";
import Collapse from "@mui/material/Collapse";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import Pagination from "@mui/material/Pagination";
import Grid from "@mui/material/Unstable_Grid2";

import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';

const API_URL = "http://localhost:12344";

let catFilters = [
  ["Produk BCA", 0],
  ["Line of Business", 0],
  ["Area Promo", 0],
];

export async function getServerSideProps(ctx) {
  const searchFor = ctx.query.searchFor==undefined?null:ctx.query.searchFor;

  return {
    props: {
      searchFor: searchFor,
    },
  };
}

const ExpandMore = styled((props) => {
  const { expand, ...other } = props;
  return <IconButton {...other} />;
})(({ theme, expand }) => ({
  transform: !expand ? "rotate(0deg)" : "rotate(180deg)",
  marginLeft: "auto",
  transition: theme.transitions.create("transform", {
    duration: theme.transitions.duration.shortest,
  }),
}));

export default function Index({ searchFor }) {
  const refReader = useRef("");

  const [expanded, setExpanded] = useState([
    false,
    false,
    false,
    false,
    false,
    false,
    false,
    false,
    false,
    false,
    false,
    false,
  ]);
  const handleExpandClick = (index) => {
    expanded[index] = !expanded[index];
    setExpanded(expanded);
    updatePromos(sort, page);
  };

  const [sort, setSort] = useState("DATE");
  const onChangeSort = (event) => {
    setSort(event.target.value);
    if (searchText != null) {
      setPage(1);
      updatePromos(event.target.value, 1);
      scroll(0, 0);
    }
  };

  const sortList = ["RELEVANCE", "DATE", "ASCENDING", "DESCENDING"];
  const displaySort = sortList.map((row) => (
    <MenuItem value={row} key={row}>
      {row}{" "}
    </MenuItem>
  ));

  const [pageCount, setPageCount] = useState(0);
  const [page, setPage] = useState(1);
  const handleChangePage = (event, newPage) => {
    setPage(newPage);
    setExpanded([
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
    ]);
    updatePromos(sort, newPage);
    scroll(0, 0);
  };

  const [searchText, setSearchText] = useState(searchFor);
  const [promos, setPromos] = useState("");
  async function updatePromos(sortBy, page) {
    try {
      const res = await fetch(
        API_URL + "?" + "itemLoad=12" + "&order=" + sortBy + "&page=" + page
      );
      const data = await res.json();
      setPromos(data);
      setPageCount(data.Result.pageInfo.PageTotal);
    } catch (err) {
      console.log(err);
    }
  }
  const onClickSearch = (input) => {
    input.preventDefault();
    setSearchText(refReader.current.value);
    setPage(1);
    setSort("DATE");
    setExpanded([
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
    ]);
    updatePromos("DATE", 1);  
    scroll(0, 0);
  };
  function displayPromos() {
    return promos.Result.listPromo.map((promo, index) => {
      return (
        <Grid lg={3} key={promo.linkPage}>
          <Card
            sx={{
              minHeight: 400,
              bgcolor: "rgb(242,255,255)",
              boxShadow: 4,
              borderRadius: 2,
            }}
          >
            <CardMedia
              component="img"
              height="194"
              image={promo.fileCover}
              alt={
                promo.namaFileCover == ""
                  ? "Image Not Found"
                  : promo.namaFileCover
              }
            />
            <CardContent>
              <Typography variant="h6" color="rgb(0, 102, 174)">
                <b>
                  <a href={"/search?link="+promo.linkPage}>
                    {promo.brand} - {promo.titlePromo.id}
                  </a>
                </b>
              </Typography>
              <Typography variant="body2" color="text.secondary">
                {promo.summaryPromo.id ==
                promo.brand + " - " + promo.titlePromo.id
                  ? ""
                  : promo.summaryPromo.id}
              </Typography>
              <br />
              <Typography variant="body2" color="text.secondary">
                {promo.startDate} - {promo.endDate}
              </Typography>
            </CardContent>
            <CardActions disableSpacing>
              <ExpandMore
                expand={expanded[index]}
                onClick={() => handleExpandClick(index)}
                aria-expanded={expanded[index]}
                aria-label="show more"
              >
                <ExpandMoreIcon />
              </ExpandMore>
            </CardActions>
            <Collapse in={expanded[index]} timeout="auto" unmountOnExit>
              <CardContent>
                <Typography>
                  <div
                    dangerouslySetInnerHTML={{ __html: promo.deskripsi.id }}
                  />
                </Typography>
              </CardContent>
            </Collapse>
          </Card>
        </Grid>
      );
    });
  }

  const [product, setProduct] = useState({
    qris: false,
    kkbca: false,
    kdbca: false,
    flazz: false,
    mybca: false,
    blu: false,
  });
  const { qris, kkbca, kdbca, flazz, mybca, blu } = product;
  const productList = [
    [qris, "qris", "QRIS"],
    [kkbca, "kkbca", "Kartu Kredit BCA"],
    [kdbca, "kdbca", "Kartu Debit BCA"],
    [flazz, "flazz", "Flazz"],
    [mybca, "mybca", "myBCA"],
    [blu, "blu", "Blu"],
  ];

  const [lob, setLob] = useState({
    ecommerce: false,
    fnb: false,
    fashion: false,
    groceries: false,
    hotel: false,
    lifestyle: false,
  });
  const { ecommerce, fnb, fashion, groceries, hotel, lifestyle } = lob;
  const lobList = [
    [ecommerce, "ecommerce", "e-Commerce"],
    [fnb, "fnb", "Food & Beverage"],
    [fashion, "fashion", "Fashion"],
    [groceries, "groceries", "Groceries"],
    [hotel, "hotel", "Hotel"],
    [lifestyle, "lifestyle", "Lifestyle"],
  ];

  const [area, setArea] = useState({
    nasional: false,
    bali: false,
    jabodetabek: false,
    jabar: false,
    jateng: false,
    jatim: false,
  });
  const { nasional, bali, jabodetabek, jabar, jateng, jatim } = area;
  const areaList = [
    [nasional, "nasional", "Nasional"],
    [bali, "bali", "Bali"],
    [jabodetabek, "jabodetabek", "Jabodetabek"],
    [jabar, "jabar", "Jawa Barat"],
    [jateng, "jateng", "Jawa Tengah"],
    [jatim, "jatim", "Jawa Timur"],
  ];

  function displayCat(cat, setCat, catList, filterType) {
    let catFilter = catFilters[filterType];
    return (
      <FormControl component="fieldset" variant="standard">
        <FormLabel component="legend">
          {catFilter[0]} {catFilter[1] === 0 ? "" : "(" + catFilter[1] + ")"}
        </FormLabel>
        <FormGroup>
          {catList.map((row) => (
            <FormControlLabel
              control={
                <Checkbox
                  checked={row[0]}
                  onChange={(event) => {
                    setCat({
                      ...cat,
                      [event.target.name]: event.target.checked,
                    });
                    if (event.target.checked) {
                      catFilter[1]++;
                    } else {
                      catFilter[1]--;
                    }
                  }}
                  name={row[1]}
                />
              }
              label={row[2]}
              key={row[1]}
            />
          ))}
        </FormGroup>
        <hr style={{ height: "20px" }} />
      </FormControl>
    );
  }

  useEffect(() => {
    updatePromos(sort, page);
  }, []);

  return (
    <Grid
      container
      style={{
        backgroundImage: `url(https://promo.bca.co.id/assets/campaign/ramadan-tenang/bg--main.jpg)`,
        backgroundSize: "contain",
      }}
    >
      <AppBar position="static" color="transparent" margin={20}>
        <Toolbar>
          <Grid
            container
            lg={12}
            style={{
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
            }}
          >
            <Grid>
              <img
                src={
                  "https://promo.bca.co.id/_next/image?url=%2F_next%2Fstatic%2Fmedia%2Flogo-bca.17c4acdc.svg&w=128&q=75"
                }
                alt="BCA Logo"
                height={50}
                width={100}
                href={"/"}
              />
            </Grid>
            <Grid sx={{ width: "57%" }}>
              <TextField
                inputRef={refReader}
                id="outline-basic"
                label="Cari Semua Promo, Yuk!"
                variant="outlined"
                fullWidth
              />
            </Grid>
            <Grid>
              <form onClick={onClickSearch}>
                <Button
                  type="submit"
                  variant="outlined"
                  endIcon={<SendIcon />}
                  sx={{ height: 56 }}
                >
                  Cari
                </Button>
              </form>
            </Grid>
          </Grid>
        </Toolbar>
      </AppBar>

      <Grid
        container
        lg={12}
        m={10}
        style={{ display: "flex", justifyContent: "center" }}
      >
        <Grid
          container
          lg={1.5}
          sx={{
            bgcolor: "rgb(242,255,255)",
            boxShadow: 4,
            borderRadius: 2,
            p: 2,
            maxHeight: 1000,
          }}
        >
          <Grid container>
            {displayCat(product, setProduct, productList, 0)}
          </Grid>
          <Grid container>{displayCat(lob, setLob, lobList, 1)}</Grid>
          <Grid container>{displayCat(area, setArea, areaList, 2)}</Grid>
        </Grid>
        <Grid lg={0.1} />
        <Grid
          container
          lg={8}
          spacing={2}
          style={{ display: "flex", justifyContent: "center" }}
        >
          <Grid
            container
            lg={12}
            style={{ display: "flex", justifyContent: "center" }}
          >
            <Grid lg={2.8}>
              <h2>
                Hasil pencarian untuk
                {searchText === null
                  ? "..."
                  : searchText === ""
                  ? " semua promo"
                  : ' "' + searchText + '"'}
              </h2>
            </Grid>
            <Grid lg={6}>
              <Pagination
                count={pageCount}
                page={page}
                onChange={handleChangePage}
                variant="outlined"
                color="primary"
                boundaryCount={2}
              />
            </Grid>
            <Grid>
              <FormControl>
                <InputLabel id="demo-simple-select-label">Sort by:</InputLabel>
                <Select
                  labelId="demo-simple-select-label"
                  id="demo-simple-select"
                  value={sort}
                  label="Sort"
                  onChange={onChangeSort}
                  sx={{ height: 40, fontSize: 12 }}
                >
                  {displaySort}
                </Select>
              </FormControl>
            </Grid>
          </Grid>
          {promos === "" ? null : displayPromos()}
          <Grid margin={1}>
            <Pagination
              count={pageCount}
              page={page}
              onChange={handleChangePage}
              variant="outlined"
              color="primary"
              boundaryCount={2}
            />
          </Grid>
        </Grid>
      </Grid>
    </Grid>
  );
}