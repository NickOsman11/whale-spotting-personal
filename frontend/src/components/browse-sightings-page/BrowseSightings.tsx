import React, { useEffect, useState } from "react";
import {
  getSightings,
  getSightingsBySpeciesId,
  getSightingsByLocationId,
  getExternalSightings,
  Sighting,
  ExternalSighting,
  GenericSighting,
  getDate,
} from "../../clients/apiClient";
import { SightingMap } from "../sighting-map/SightingMap";
import { SightingList } from "../sighting-list/SightingList";
import "./BrowseSightings.scss";
import { useLocation, useParams } from "react-router-dom";
import { compareDesc } from "date-fns";
import { BrowseSightingsControls } from "./BrowseSightingsContols";

export const BrowseSightings: React.FC = () => {
  const [sightings, setSightings] = useState<Sighting[]>();
  const [externalSightings, setExternalSightings] =
    useState<ExternalSighting[]>();
  const [isShowingMap, setIsShowingMap] = useState(false);
  const query = new URLSearchParams(useLocation().search);
  query.append("locationid", "2");
  console.log(query.toString());
  const { speciesId, locationId } = useParams<{
    speciesId: string;
    locationId: string;
  }>();

  let allSightings: GenericSighting[] = (
    (sightings as GenericSighting[]) ?? []
  ).concat(...(externalSightings ?? []));

  allSightings = allSightings.sort((a, b) =>
    compareDesc(getDate(a), getDate(b))
  );

  useEffect(() => {
    if (speciesId !== undefined) {
      getSightingsBySpeciesId(speciesId).then(setSightings);
    } else if (locationId !== undefined) {
      console.log(locationId);
      getSightingsByLocationId(locationId).then((sightings) =>
        setSightings(sightings)
      );
    } else {
      getSightings().then((sightings) => setSightings(sightings));
      getExternalSightings().then(setExternalSightings);
    }
  }, [speciesId, locationId]);

  let contents = <></>;

  if (sightings === undefined && externalSightings === undefined) {
    contents = <p>Loading...</p>;
  } else if (sightings && speciesId && sightings.length === 0) {
    contents = <p>Sorry, we have no sightings of that species!</p>;
  } else if (sightings && locationId && sightings.length === 0) {
    contents = <p>Sorry, we have no sightings from that location!</p>;
  } else if (sightings && externalSightings && allSightings.length === 0) {
    contents = <p>Sorry, there are no sightings to display.</p>;
  } else if (
    sightings &&
    externalSightings === undefined &&
    allSightings.length === 0
  ) {
    contents = (
      <>
        <p>Sorry, there are no sightings to display.</p>
        <p>Loading sightings from the Washington Whale Hotline...</p>
      </>
    );
  } else {
    contents = (
      <>
        {isShowingMap ? (
          <SightingMap sightings={allSightings} />
        ) : (
          <SightingList sightings={allSightings} />
        )}
      </>
    );
  }

  return (
    <>
      <h1>Reported Sightings</h1>
      <BrowseSightingsControls
        isShowingMap={isShowingMap}
        setIsShowingMap={setIsShowingMap}
      />
      {contents}
    </>
  );
};
