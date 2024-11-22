import { observer } from 'mobx-react-lite'
import Calendar from 'react-calendar'
import { Header, Menu } from 'semantic-ui-react'
import { useMainStore } from '../../../app/stores/mainStore'
import ActivityRelationship from '../../../app/models/activityRelationship'

export default observer(function ActivityFilters() {
  const { activityStore } = useMainStore()
  const { listFilters, setListFilter } = activityStore

  return (
    <>
      <Menu vertical size="large" style={{ width: '100%', marginTop: 27 }}>
        <Header icon="filter" attached color="teal" content="Filters" />
        <Menu.Item
          content="All Activities"
          active={listFilters.relationship == ActivityRelationship.None}
          onClick={() =>
            setListFilter('relationship', ActivityRelationship.None)
          }
        />
        <Menu.Item
          content="I'm going"
          active={listFilters.relationship == ActivityRelationship.IsGoing}
          onClick={() =>
            setListFilter('relationship', ActivityRelationship.IsGoing)
          }
        />
        <Menu.Item
          content="I'm hosting"
          active={listFilters.relationship == ActivityRelationship.IsHost}
          onClick={() =>
            setListFilter('relationship', ActivityRelationship.IsHost)
          }
        />
      </Menu>
      <Header />
      <Calendar
        value={listFilters.fromDate}
        onChange={(date) => setListFilter('fromDate', date as Date)}
      />
    </>
  )
})
