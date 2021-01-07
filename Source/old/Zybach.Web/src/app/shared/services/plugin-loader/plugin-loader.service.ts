import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';




@Injectable({
  providedIn: 'root'
})
export class PluginLoaderService {

  private plugins = {
    echarts: {
      loaded: false,
      src: [
        'assets/plugins/echarts/echarts.min.js',
      ],
    },
    leaflet: {
      loaded: false,
      src: [
        'https://unpkg.com/leaflet@1.2.0/dist/leaflet.js',
        'https://cdnjs.cloudflare.com/ajax/libs/leaflet.draw/1.0.4/leaflet.draw.js',
        'https://cdnjs.cloudflare.com/ajax/libs/Leaflet.awesome-markers/2.0.2/leaflet.awesome-markers.min.js',
        'assets/plugins/leaflet/tile-layer-offline.js',
        'assets/plugins/leaflet/leaflet-bing-layer.js',
        'assets/plugins/leaflet/leaflet.fullscreen.js',
      ],
    },
    tableau: {
      loaded: false,
      src: [
        'https://connectors.tableau.com/libs/tableauwdc-2.3.latest.js',
      ],
    },

  };

  constructor() {

  }

  load(pluginKey: string): Observable<boolean> {
    const subject = new Subject<boolean>();

    const plugin = this.plugins[pluginKey];
    if (!plugin.loaded) {
      plugin.loaded = true;

      const hostElement = document.getElementById('plugin-scripts');

      if (typeof plugin.src === 'string') {
        this.loadScripts(hostElement, [plugin.src], subject);
      } else {
        this.loadScripts(hostElement, plugin.src, subject);
      }
    } else {
      setTimeout(() => {
        subject.next(true);
        subject.complete();
      });
    }

    return subject.asObservable();
  }

  private loadScripts(hostElement: any, scripts: string[], subject: Subject<boolean>): void {
    if ((scripts || []).length > 0) {
      const src = scripts.splice(0, 1)[0];
      const scriptElement = document.createElement('script');
      scriptElement.type = 'text/javascript';
      scriptElement.src = src;
      scriptElement.onload = () => {
        console.log(src);
        this.loadScripts(hostElement, scripts, subject);
      }
      scriptElement.onerror = (error: any) => {
        console.error(`error loading ${src}`);
        subject.next(false);
        subject.complete();
      };
      hostElement.appendChild(scriptElement);
    } else {
      subject.next(true);
      subject.complete();
    }
  }
}
