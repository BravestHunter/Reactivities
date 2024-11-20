import { makeAutoObservable } from 'mobx'
import ActivityRelationship from './activityRelationship'

export interface ActivityListFilters {
  fromDate: Date
  relationship: ActivityRelationship
}

export class ActivityListFilters implements ActivityListFilters {
  fromDate: Date = new Date()
  relationship: ActivityRelationship = ActivityRelationship.None

  constructor() {
    makeAutoObservable(this)
  }
}
