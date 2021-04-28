import mongoose, { Schema, Document } from "mongoose";
import {Geometry} from "geojson";

export interface StreamFlowZoneInterface extends Document {
    type: string;
    properties: {
        FeatureID: number,
        ZoneName: string,
        Length: number,
        Area: number
    };
    geometry: Geometry;
}

const StreamFlowZonePropertiesSchema = new Schema({
    FeatureID: {
        type: Number
    },
    ZoneName: {
        type: String
    },
    Length: {
        type: Number
    },
    Area: {
        type: Number
    }
});

const StreamFlowZoneSchema = new Schema({
  type: {
      type: String,
  },
  properties: {
      type: StreamFlowZonePropertiesSchema
  },
  geometry: {
      type: {
          type: String
      },
      coordinates: {
          // Mongoose seems to have problems with the [[[Number]]] type that would be the correct type for this field
          // fortunately, we can still set geometry: Geometry on the interface
          type: Schema.Types.Mixed
      }
  }
});

const StreamFlowZone = mongoose.model<StreamFlowZoneInterface>("StreamFlowZone", StreamFlowZoneSchema, "StreamFlowZone");

export default StreamFlowZone;