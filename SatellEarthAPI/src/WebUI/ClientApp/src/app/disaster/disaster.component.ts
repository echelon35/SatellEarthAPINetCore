import { Component, OnInit } from '@angular/core';
import { AleaDto, AleasClient, DisasterDto, DisastersClient } from '../web-api-client';

@Component({
  selector: 'app-disaster',
  templateUrl: './disaster.component.html',
  styleUrls: ['./disaster.component.scss']
})
export class DisasterComponent implements OnInit {
  debug = false;
  aleaList: AleaDto[];
  disasterList: DisasterDto[];
  selectedDisaster: DisasterDto;
  selectedAlea: AleaDto;

  constructor(private aleasClient: AleasClient, private disastersClient: DisastersClient) { }

  ngOnInit(): void {
    this.aleasClient.get().subscribe(
      result => {
        console.log(result);
        this.aleaList = result.lists;
        if (this.aleaList.length) {
          this.selectedAlea = this.aleaList[0];
        }
      },
      error => console.error(error)
    );
  }

}
