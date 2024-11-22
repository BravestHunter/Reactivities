import { makeAutoObservable } from 'mobx'
import ActivityRelationship from './activityRelationship'

export const maxDate = new Date('3000-01-01T00:00:00')
export const minDate = new Date('2020-01-01T00:00:00')

export interface ActivityListFilters {
  fromDate: Date
  toDate: Date
  relationship: ActivityRelationship
}

export class ActivityListFilters implements ActivityListFilters {
  fromDate: Date = new Date()
  toDate: Date = maxDate
  relationship: ActivityRelationship = ActivityRelationship.None

  constructor() {
    makeAutoObservable(this)
  }
}
