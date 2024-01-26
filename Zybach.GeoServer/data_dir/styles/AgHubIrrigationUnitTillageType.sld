<?xml version="1.0" encoding="ISO-8859-1"?>
<StyledLayerDescriptor version="1.0.0"
                       xsi:schemaLocation="http://www.opengis.net/sld StyledLayerDescriptor.xsd"
                       xmlns="http://www.opengis.net/sld"
                       xmlns:ogc="http://www.opengis.net/ogc"
                       xmlns:xlink="http://www.w3.org/1999/xlink"
                       xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <!-- a Named Layer is the basic building block of an SLD document -->
  <NamedLayer>
    <Name>Irrigation Unit Tillage Type</Name>
    <UserStyle>
      <!-- Styles can have names, titles and abstracts -->
      <Title>Irrigation Unit Tillage Type</Title>
      <!-- FeatureTypeStyles describe how to render different features -->
      <!-- A FeatureTypeStyle for rendering polygons -->
      <FeatureTypeStyle>
        <Rule>
          <Name>rule1</Name>
          <Title>Style for ESU Layer</Title>
          <Abstract>ESU Style</Abstract>
          <PolygonSymbolizer>
            <Fill>
              <CssParameter name="fill">
                <ogc:PropertyName>TillageTypeMapColor</ogc:PropertyName>
              </CssParameter>
              <CssParameter name="fill-opacity">.65</CssParameter>
            </Fill>
          </PolygonSymbolizer>
        </Rule>
      </FeatureTypeStyle>
    </UserStyle>
  </NamedLayer>
</StyledLayerDescriptor>