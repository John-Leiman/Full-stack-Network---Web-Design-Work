import * as React from "react";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import SendIcon from "@mui/icons-material/Send";

import { styled } from "@mui/material/styles";
import Card from "@mui/material/Card";
import CardMedia from "@mui/material/CardMedia";
import CardContent from "@mui/material/CardContent";
import CardActions from "@mui/material/CardActions";
import Collapse from "@mui/material/Collapse";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import Grid from "@mui/material/Unstable_Grid2";

import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";

import { useState, useRef, useEffect } from "react";

// @ts-check

/** @type {string} - API_URL */
let API_URL = "http://localhost:12344";

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

export async function getServerSideProps(ctx) {
  /** @type {string} */
  const linkPage = ctx.query.link;

  return {
    props: {
      linkPage: linkPage,
    },
  };
}

export default function search({ linkPage }) {
  const refReader = useRef(/** @type {string} */ "");
  const [searchText, setSearchText] = useState(/** @type {string} */ "");
  const [promo, setPromo] = useState(/** @type {Object} */ null);
  const [lob, setLOB] = useState(/** @type {string} */ "");
  const [recs, setRecs] = useState(/** @type {Object} */ null);

  async function searchPromo() {
    try {
      const res = await fetch(
        API_URL + "/PromoVisitor/getLinkPromo?searchId=" + linkPage
      );
      /** @type {Object} */
      const data = await res.json();
      setPromo(data);
      setLOB(data.Result.lob);
    } catch (err) {
      console.log(err);
    }
  }

  async function searchRecs() {
    try {
      /** @type {Response} */
      let res = await fetch(
        API_URL + "/PromoVisitor/getRecPromos?searchId=" + linkPage
      );
      /** @type {Response} */
      let data = await res.json();
      setRecs(data);
    } catch (err) {
      console.log(err);
    }
  }

  useEffect(() => {
    searchPromo();
    searchRecs();
  }, []);

  const [expanded, setExpanded] = useState(
    /** @type {Boolean[]} */ [false, false, false, false]
  );

  const handleExpandClick = (index) => {
    expanded[index] = !expanded[index];
    searchRecs();
  };

  function displayPromo() {
    return (
      <Grid container>
        <Grid>
          <Typography
            variant="h3"
            color="rgb(0, 102, 174)"
            sx={{ fontWeight: "bold" }}
          >
            {promo.Result.brand} - {promo.Result.titlePromo.id}
          </Typography>
          <Typography variant="body1" color="rgb(71, 71, 71)">
            {promo.Result.startDate} - {promo.Result.endDate}
          </Typography>
        </Grid>
        <Grid
          container
          sx={{
            display: "flex",
            justifyContent: "center",
          }}
        >
          <Grid
            margin={1}
            sx={{
              display: "flex",
              justifyContent: "center",
            }}
          >
            <img
              src={promo.Result.fileCover}
              alt={promo.Result.namaFileCover}
              style={{
                borderRadius: "15px",
                display: "block",
                minWidth: 1280,
              }}
            />
          </Grid>
          <Grid
            lg={12}
            padding={5}
            sx={{
              minHeight: 420,
              minWidth: 1280,
              bgcolor: "rgb(242,255,255)",
              boxShadow: 4,
              borderRadius: 4,
            }}
          >
            <Typography variant="h5" color="rgb(71, 71, 71)">
              Bagi Pengguna
            </Typography>
            <Typography variant="body1" color="rgb(0, 102, 174)">
              <a href={"/"}>
                {promo.Result.lob === ""
                  ? "- Unredeemable"
                  : promo.Result.lob}
              </a>
              <br />
              <br />
            </Typography>
            <div
              dangerouslySetInnerHTML={{ __html: promo.Result.deskripsi.id }}
            />
          </Grid>
        </Grid>
      </Grid>
    );
  }

  function displayRecs() {
    return recs.Result.map((rec, index) => {
      return (
        <Grid lg={3} key={rec.linkPage}>
          <Card
            sx={{
              minHeight: 420,
              bgcolor: "rgb(242,255,255)",
              boxShadow: 4,
              borderRadius: 2,
            }}
          >
            <CardMedia
              component="img"
              height="194"
              image={rec.fileCover}
              alt={
                rec.namaFileCover === "" ? "Image Not Found" : rec.namaFileCover
              }
            />
            <CardContent>
              <Typography variant="h6" color="rgb(0, 102, 174)">
                <b>
                  <a href={"/search?link=" + rec.linkPage}>
                    {rec.brand} - {rec.titlePromo.id}
                  </a>
                </b>
              </Typography>
              <Typography variant="body2" color="text.secondary">
                {rec.summaryPromo.id === rec.brand + " - " + rec.titlePromo.id
                  ? ""
                  : rec.summaryPromo.id}
              </Typography>
              <br />
              <Typography variant="body2" color="text.secondary">
                LOB: {rec.lob === "" ? "Undeclared" : rec.lob}
              </Typography>
              {/* <br />
              <Typography variant="body2" color="text.secondary">
                {rec.startDate} - {rec.endDate}
              </Typography> */}
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
                <div dangerouslySetInnerHTML={{ __html: rec.deskripsi.id }} />
              </CardContent>
            </Collapse>
          </Card>
        </Grid>
      );
    });
  }

  return (
    <Grid
      container
      sx={{
        backgroundImage: `url(https://promo.bca.co.id/assets/campaign/ramadan-tenang/bg--main.jpg)`,
        backgroundSize: "contain",
      }}
    >
      <AppBar position="static" color="transparent">
        <Toolbar>
          <Grid
            container
            lg={12}
            sx={{
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
              />
            </Grid>
            <Grid sx={{ width: "57%" }}>
              <TextField
                inputRef={refReader}
                id="outline-basic"
                label="Cari Semua Promo, Yuk!"
                variant="outlined"
                onChange={(event) => {
                  setSearchText(event.target.value);
                }}
                fullWidth
              />
            </Grid>
            <Grid>
              <Button
                type="submit"
                variant="outlined"
                endIcon={<SendIcon />}
                href={"/?searchFor=".concat(searchText)}
                sx={{ height: 56 }}
              >
                Cari
              </Button>
            </Grid>
          </Grid>
        </Toolbar>
      </AppBar>

      <Grid container mx={40} my={10}>
        <Grid container lg={12} spacing={2}>
          <Grid lg={12}></Grid>
          {promo == null ? null : displayPromo()}
          <Grid lg={12} mt={5}>
            <Typography variant="h5" color="rgb(71, 71, 71)">
              Promo Serupa (LOB: {lob === "" ? "Undeclared" : lob})
            </Typography>
          </Grid>
          {recs == null ? null : displayRecs()}
        </Grid>
      </Grid>
    </Grid>
  );
}
